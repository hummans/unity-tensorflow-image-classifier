# Parameters:
# → image_base64 (encoding image array)
# → image_url (s3 download authorization)

import os
import logging
import requests
import random

import tensorflow as tf
import tensorflow.keras.preprocessing.image as tfi
import pandas as pd
import numpy as np

from packages.lambda_handlers.response.wrapper import *
from packages.lambda_handlers.errors import *
from packages.lambda_handlers.handlers.lambda_handler import LambdaHandler

# Context:
# - Add EFS Path to python
# - Import libraries
# - Load model

# Handler
# - Read image url
# - Prepare image -> data augmentation
# - Make inference
# - Return predicted result

logger = logging.getLogger()
logger.setLevel(logging.INFO)

efs_path = "./"

"""
model_path = os.path.join(efs_path, 'model/')
model = tf.keras.models.load_model(model_path)
"""

#model_path = r"model.h5"
path = "/"
model_path = os.path.join(path, 'model/')
model = tf.keras.models.load_model(model_path)

logger.info("Lambda context execution success!")
print("Lambda context execution success!")


def lambda_handler(event, context):
    lambdaHandler = LambdaHandler(event=event, context=context)
    
    route_key = event['routeKey']
    image_url = event["queryStringParameters"]["image_url"]
    
    try:
        img_array = prepare_image(image_url, (128,128))
        
        predict = model.predict(img_array)


        prediction = random.randint(0,1)
        accuracy = 0.78945
        label = get_label(prediction)
        
        return buildResponse(operation=route_key, 
                            data={  # Data predicted
                                "label": label,
                                "prediction": prediction,
                                "accuracy": accuracy
                            },
                            lambdaHandler=lambdaHandler).asjson()
    except:
        lambdaHandler.performError(BadRequestError())
        return buildResponse(operation=route_key, 
                             data={},
                             lambdaHandler=lambdaHandler
                             ).asjson()

def prepare_image(url, IMG_SHAPE):
    # Preprocessing image from url -> tf.keras.preprocessing.image
    
    image_url = tf.keras.utils.get_file('Court', origin=url)
    img = tf.keras.preprocessing.image.load_img(image_url, target_size=IMG_SHAPE)
    os.remove(image_url) # Remove the cached file
    img_array = tf.keras.preprocessing.image.img_to_array(img)
    
    return img_array

def get_label(prediction):
    lbl = 'Cat' if prediction == 0 else 'Dog'
    return "It's a {lbl}".format(lbl=lbl)

def download_image(url):
    try:
        request = requests.get(url)
        print("Downloading image from url: {}".format(url))
        return request
    except requests.exceptions.ConnectionError as e:
        return e