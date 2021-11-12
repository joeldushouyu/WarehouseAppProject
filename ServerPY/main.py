
from User import  User
from Database import db
db.create_all()
u = User(accountName="Admin",accountType="Manager",password="none")
db.session.add(u)
db.session.commit()
