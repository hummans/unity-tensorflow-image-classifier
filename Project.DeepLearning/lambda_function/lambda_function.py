import requests
import base64

from lambda_handlers.errors import getDataFailed
from lambda_handlers.response.wrapper import *

# Parameters:
# - image_url -> s3 download url auth

def lambda_handler(event, context):
    route_key = event['routeKey']
    image_url = event["queryStringParameters"]["image_url"]
    return predict_url(route_key, image_url)

def predict_url(route_key, image_url):
    try:
        # 1. Download image
        image = download_image(image_url)
        # 2. Predict image type
        # 3. Predict image color
        return buildResponse(operation=route_key, 
                            data={
                                "image":base64.b64encode(image.content)
                            },
                            count=1
                            )
    except:
        error = getDataFailed().toLambda()
        return buildResponse(operation="routeKey", 
                             data={},
                             count=0,
                             lambdaError=error
                             ) 

def download_image(url):
    request = requests.get(url, stream=True)
    print("Downloading image from url: {}".format(url))
    return request