from ctypes import Array
import pytest
import requests
import pandas as pd
import numpy as np

from packages.classify_cat_dog import download_image, get_label, prepare_image

class TestClassifyCatDog:
    
    @pytest.fixture
    def data_image(self):
        dir = r""
        data = pd.DataFrame({
            'image_shape': (128,128),
            'directory': dir
        })
        return data
    
    @pytest.fixture
    def valid_url(self):
        return "https://i.pinimg.com/originals/87/fd/4e/87fd4e9caf9bd711039faba32ae6e7f4.jpg"
    
    @pytest.fixture
    def unvalid_url(self):
        return "https://i.pinimgcom/"
    
    def test_get_label_dog(self):
        lbl = get_label(1)
        assert lbl == "It's a Dog"
        
    def test_get_label_cat(self):
        lbl = get_label(0)
        assert lbl == "It's a Cat"
    
    def test_download_image_sucess(self, valid_url):
        r = download_image(valid_url)
        
        assert r.status_code == 200
        assert r.content is not None
    
    def test_download_image_failed(self, unvalid_url):
        r = download_image(unvalid_url)
        assert isinstance(r, requests.exceptions.ConnectionError)
    
    def test_prepare_image(self, valid_url):
        img_array = prepare_image(valid_url, (128,128))
        
        assert img_array is not None
        assert isinstance(img_array, np.ndarray)