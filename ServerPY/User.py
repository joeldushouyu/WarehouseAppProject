
from Enumerables.AccountType import AccountType

from Database import db,orm

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    accountName = db.Column(db.String(100), nullable=False)
    accountType = db.Column(db.String(100), nullable=False)
    password = db.Column(db.String(100), nullable=False)


    @orm.reconstructor
    def __init__(self, **kwargs):
        super(User, self).__init__(**kwargs)
        self.pickedUpOrders= []  # list of orders picked up by users
        self.sessionId = ""  # current active user UUID generate from login


