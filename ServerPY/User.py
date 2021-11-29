
from Enumerables.AccountType import AccountType

from Database import db,orm

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    accountName = db.Column(db.String(100), nullable=False)
    accountType = db.Column(db.String(100), nullable=False)
    password = db.Column(db.String(100), nullable=False)
    #workingSection = "AA-AA"  # default
    #pickedUpOrders = []
    #sessionId = ""

    @orm.reconstructor
    def __init__(self, **kwargs):
        super(User, self).__init__(**kwargs)
        self.pickedUpOrders= []
        self.sessionId = ""
        #self.currentActiveSession = None

    """
    def __init__(self, workingSection, sessionId, currentActiveSession):

        self.workingSection = workingSection
        self.sessionId = sessionId # the current uuid that user logined
        self.currentActiveSession = currentActiveSession # the current activeSession that holds the Command going to be executed

        self.PickedUpOrders = []   # List indicate orders being picked up by this current user

    """
