import requests
import base64
import random

from packages.lambda_handlers.errors import getDataFailed
from packages.lambda_handlers.response.wrapper import *

# Parameters:
# → image_base64 (encoding image array)
# → image_url (s3 download authorization)

def lambda_handler(event, context):
    route_key = event['routeKey']
    image_base64 = event["queryStringParameters"]["image_base64"]
    image_url = event["queryStringParameters"]["image_url"]
    
    response = predict_url(route_key, image_url)
    return response.asjson()
    
def predict_url(route_key, image_url):
    try:
        # 1. Predict image type
        prediction = random.randint(0,1)
        accuracy = 0.78945
        label = get_label(prediction)
        
        return buildResponse(operation=route_key, 
                            data={  # Data predicted
                                "label": label,
                                "prediction": prediction,
                                "accuracy": accuracy
                            },
                            count=1)
    except:
        error = getDataFailed().toLambda()
        return buildResponse(operation="routeKey", 
                             data={},
                             count=0,
                             lambdaError=error
                             ) 

def get_label(prediction):
    lbl = 'Cat' if prediction == 0 else 'Dog'
    return "It's a {lbl}".format(lbl=lbl)


def download_image(url):
    request = requests.get(url, stream=True)
    print("Downloading image from url: {}".format(url))
    return request