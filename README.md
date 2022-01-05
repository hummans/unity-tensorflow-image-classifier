<h1 align="center"> Image Classifier (Unity + Deep Learning) </h1>
  
  <div align="center">

  [![GitHub release (latest by date)](https://img.shields.io/github/v/release/matiasvallejosdev/unity-tensorflow-image-classifier?color=4cc51e)](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier)
  [![GitHub top language](https://img.shields.io/github/languages/top/matiasvallejosdev/unity-tensorflow-image-classifier?color=1081c2)](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier/search?l=c%23)
  [![GitHub Watchers](https://img.shields.io/github/watchers/matiasvallejosdev/unity-tensorflow-image-classifier?color=4cc51e)](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier/watchers)
  [![GitHub Repo stars](https://img.shields.io/github/stars/matiasvallejosdev/unity-tensorflow-image-classifier?color=4cc51e)](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier/stargazers)
  [![GitHub Forks](https://img.shields.io/github/forks/matiasvallejosdev/unity-tensorflow-image-classifier?color=4cc51e)](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier/network/members)
  <br />
  [![Unity Badge](http://img.shields.io/badge/-Unity3D_2020.3.5f1-000?logo=unity&link=https://unity.com/)](https://unity.com/)
  [![made-for-VSCode](https://img.shields.io/badge/Made%20for-VSCode-1f425f.svg)](https://code.visualstudio.com/)
  [![made-for-Colab](https://img.shields.io/badge/Made%20for-Colab-orange)](https://colab.research.google.com/)
  </div>
  
  <p align="center"> <br />
This repository contains an interactive augmented reality application developed in Unity that uses a neural network hosted on AWS to identify the image and colors. Once classified, the classified object in the image is shown on stage using AR Foundation augmented reality markers (ARCore - ARKit). <br />
  </p>
    
  </p>
</p>

## Table of Contents

- [Table of Contents](#table-of-contents)
- [Installation](#installation)
- [Arquitecture](#arquitecture)
- [Contributing](#contributing)
- [Screenshoot](#screenshoot)
- [Data Source](#data-source)
- [Credits](#credits)
- [Thanks](#thanks)

## Installation
　1. Clone a repository or download it as zip.
```
    git clone https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier
```
## Arquitecture

EFS to store deep learning (DL) framework libraries and models to load from Lambda to execute inferences. We provide a code example on executing serverless inferences with TensorFlow 2.

### Diagram
This is a picture of the servereless machine learning inference architecture and the execution flow.

![Diagram](https://github.com/matiasvallejosdev/unity-tensorflow-image-classifier/blob/main/Project.Backend/docs/Serverless_Diagram.png?raw=true)

You can access to more information in this [article](https://aws.amazon.com/blogs/compute/building-deep-learning-inference-with-aws-lambda-and-amazon-efs/) developed by [James Beswick](https://aws.amazon.com/blogs/compute/author/jbeswick/).
### Prerequisites

1. Storing the deep learning libraries and model on Amazon EFS
2. Creating a Lambda function for inference

## Screenshoot
Ar Application Screenshoot on Android Device.
<p>
  <p>
    <a rel="nofollow">
    <img src="" width="200">
    </a>
    <a rel="nofollow">
    <img src="" width="200">
    </a>
    <a rel="nofollow">
    <img src="" width="200">
    </a>
  </p>

## Data Source

The algorithm used is based on the data from the Kaggle competition. The following [dataset](https://www.kaggle.com/c/dogs-vs-cats/data) were used:
- test1.zip
- train.zip


It also bases the algorithm designed under the name [AlexNet](https://en.wikipedia.org/wiki/AlexNet) mounted on Tensorflow.



## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are greatly appreciated. <br /><br />
　1.　Fork the Project. <br />
　2.　Create your Feature Branch. <br />
　3.　Commit your Changes. <br />
　4.　Push to the Branch. <br />
　5.　Open a Pull Request. <br />

## Credits

- Main Developer: [Matias A. Vallejos](https://www.linkedin.com/in/matiasvallejos/)

## Thanks

_For more information about the project contact me! Do not hesitate to write me just do it!_
