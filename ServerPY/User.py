
from Enumerables import AccountType


class User():

    def __init__(self, id, accountName:str, workingSection, sessionId, currentActiveSession, accountType:AccountType):
        self.id = id
        self.accountName =accountName
        self.workingSection = workingSection
        self.sessionId = sessionId # the current session that user logined
        self.currentActiveSession = currentActiveSession # the current activeSession that holds the Command going to be executed
        self.accountType = accountType
        self.PickedUpOrders = []   # List indicate orders being picked up by this current user




