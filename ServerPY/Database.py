
from flask import Flask, request, abort, jsonify, Response
from flask_sqlalchemy import  SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///products.sqlite3'


db = SQLAlchemy(app)
db.create_all()