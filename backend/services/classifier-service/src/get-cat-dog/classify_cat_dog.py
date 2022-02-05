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

from lambda_handlers.response.wrapper import *
from lambda_handlers.errors import *
from lambda_handlers.handlers.http_handler import HTTPHandler
from lambda_handlers.handlers.event_handler import EventHandler
from lambda_handlers.errors import LambdaError, InternalServerError

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

"""
efs_path = "./"
model_path = os.path.join(efs_path, 'model/')
model = tf.keras.models.load_model(model_path)
"""

#model_path = os.path.join(path, 'model/')
model_path = r"C:\Users\matia\Desktop\.Temp\dogs-vs-cats\model.h5"
model = tf.keras.models.load_model(model_path)

logger.info("Lambda context execution success!")
print("Lambda context execution success!")

@HTTPHandler(CORSHeaders(origin='localhost', credentials=False))
def lambda_function(event, context):
    handler = EventHandler(event, context)
        
    route_key = event['routeKey']
    image_url = event["queryStringParameters"]["image_url"]
    
    response ={
            "label": "label",
            "prediction": 0,
            "accuracy": 0.75 
        } 
    
    img = prepare_image(image_url, (128,128))
    
    predict = model.predict(img[None,:,:])
    score = tf.nn.softmax(predict[0])
    accuracy = 100 * np.max(score)
    label = get_label(score)
    return buildLambdaBody(route_key, response)      

def prepare_image(url, IMG_SHAPE):
    # Preprocessing image from url -> tf.keras.preprocessing.image
    
    image_url = tf.keras.utils.get_file('Court', origin=url)
    img = tf.keras.preprocessing.image.load_img(image_url, target_size=IMG_SHAPE)
    #img = tf.keras.preprocessing.image.load_img(image_url)
    os.remove(image_url) # Remove the cached file
    array = tf.keras.preprocessing.image.img_to_array(img)

    print(array.shape)
    return array

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

def event_f():
    PATH = r"C:\Users\matia\Desktop\Matias A. Vallejos\Github\Github.Personal\unity-tensorflow-image-classifier\backend\services\classifier-service\src\get-cat-dog\event.json"
    event = {}
    with open(PATH, 'r') as f:
        event = json.load(f)
        event = json.loads(event["event_sample_image"]) 
    return event