from LocationComponent import LocationComponent
from PIL import Image
from Notification import  Notification
from Enumerables.NotificationType import  NotificationType
import io
from Database import db as db
import  os

class Item(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    itemBarcode = db.Column(db.Integer, nullable=False)   # note.
    image = db.Column(db.String(100000), nullable=True)  # picture in binray
    quantity = db.Column(db.Integer, nullable=False)
    notificationType = db.Column(db.String(100), nullable=True)
    notificationDate = db.Column(db.String(100), nullable=True)
    weight = db.Column(db.Float, nullable=True)
    locationID = db.Column(db.Integer, nullable=False)

    def to_dict(self):
        return {"id":self.id, "itemBarcode":self.itemBarcode, "image":self.image, "quantity":self.quantity,
                "LocationID": self.locationID, "notificationDate":self.notificationDate, "notificationType":self.notificationType}
    #https://stackoverflow.com/questions/33101935/convert-pil-image-to-byte-array
    def convert_picture_to_byte_array(self):
        imgByteArr = io.BytesIO()
        roi_img = self.image
        roi_img.save(imgByteArr, format=self.image.format)
        imgByteArr = imgByteArr.getvalue()
        return imgByteArr


""""
image = Image.open(os.path.join(os.getcwd(),"pictures","stationary", "MemberMarkHandSantilizer.png"))

image.thumbnail((500,500), Image.ANTIALIAS)
print(image)
def convert_picture_to_byte_array():
    imgByteArr = io.BytesIO()
    roi_img = image
    roi_img.save(imgByteArr, format=image.format)
    imgByteArr = imgByteArr.getvalue()
    return imgByteArr

"""