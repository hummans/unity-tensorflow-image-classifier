import json

from lambda_function.lambda_function import lambda_handler
from package.lambda_handlers.response.wrapper import *

from typing import NewType

"""Config this path before execute the script"""
PATH_FILE = r'C:\Users\matia\Desktop\Matias A. Vallejos\Github\Github.Personal\Personal.Unity\AR-image-classifier\Project.DeepLearning\config\config.json'
event_json = {}
with open(PATH_FILE) as f:
  event_json = json.load(f)

event = json.loads(event_json['event_sample_image'])
context = NewType('LambdaContext', object)

print(lambda_handler(event, context))