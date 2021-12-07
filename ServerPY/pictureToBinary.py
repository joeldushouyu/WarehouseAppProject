import io
from PIL import Image
import os
from Item import Item
from ServerModel import  db

print(os.getcwd())
image = Image.open(os.path.join(os.getcwd(),"pictures","food", "IceBreakersStrawberry.png"))
def convert_picture_to_byte_array():
    imgByteArr = io.BytesIO()
    roi_img = image
    roi_img.save(imgByteArr, format=image.format)
    imgByteArr = imgByteArr.getvalue()
    return imgByteArr
