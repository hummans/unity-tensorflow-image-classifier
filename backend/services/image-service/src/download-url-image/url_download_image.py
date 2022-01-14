import json
import boto3
from botocore.client import Config

def lambda_handler(event, context):
    
    key = 'user_screenshoot/' + event['queryStringParameters']['key']
    expiresin = event['queryStringParameters']['expiresin']
    
    url = boto3.client('s3').generate_presigned_url(
        ClientMethod='get_object',
        Params={'Bucket': 'color-mapping-transfer' , 'Key': key},
        ExpiresIn=expiresin
        )
        
    return {
        "statusCode": 200,
        "headers":{
            "Content-Type": "application/json"
        },
        "body": json.dumps({'url': url})
    }
