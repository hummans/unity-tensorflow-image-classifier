import json
from os import name

# Config this path before execute this script
PATH_FILE = r'C:\Users\matia\Desktop\Matias A. Vallejos\Github\Github.Personal\Personal.Unity\AR-image-classifier\Project.DeepLearning\config'
NAME_FILE = 'config'

event_get_all= {
  "routeKey": "GET /classify",
  "pathParameters": {},
  "body": {},
  "queryStringParameters": {
    "image_url": "https://es.acervolima.com/wp-content/uploads/2021/09/acervo-lima-logo-250x28.png"
  }
}

event = {
    'event_sample_image':  json.dumps(event_get_all)
}

def save_json_dict(path, name, dict):
    with open('{path}\{name}.json'.format(name=name, path=path), 'w') as f:
        json.dump(dict, f)

    print(json.dumps(dict, indent=2))

save_json_dict(PATH_FILE, NAME_FILE, event)

