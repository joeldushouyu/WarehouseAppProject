# To use this,
# pip install flask_sqlalchemy

from flask import Flask, request, abort, jsonify, Response
from werkzeug.exceptions import HTTPException
import json
import time
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///products.sqlite3'
db = SQLAlchemy(app)

# Define model class
class LocationInformation(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    description = db.Column(db.String(80), nullable=False)
    price = db.Column(db.Float, nullable=False)

    def to_dict(self):
        # See https://stackoverflow.com/questions/7102754/jsonify-a-sqlalchemy-result-set-in-flask for a general solution
        return { 'id': self.id, 'description': self.description, 'price': self.price}


db.create_all() # Create tables from model classes



p = LocationInformation(description="", price=1)
db.session.add(p)
db.session.commit()