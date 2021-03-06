from LocationComponent import LocationComponent
from PIL import Image
from Notification import  Notification
from Enumerables.NotificationType import  NotificationType
import io
from Database import  db


#https://stackoverflow.com/questions/1167617/in-python-how-do-i-indicate-im-overriding-a-method
def overrides(interface_class):
    def overrider(method):
        assert(method.__name__ in dir(interface_class))
        return method
    return overrider

class LocationItem(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    itemBarcode = db.Column(db.Integer, nullable=True)
    sectionAndLocation = db.Column(db.String(100), nullable=False)
            # this variable indicate the location of the locationitem
            # Ex: AB, indicate is in Section AB of the warehouse



    @overrides(LocationComponent)
    def add(self, loc):
        raise Exception("Function is not callable in LocationItem")


    @overrides(LocationComponent)
    def remove(self, loc):
        raise Exception("Function is not callable in LocationITem")


