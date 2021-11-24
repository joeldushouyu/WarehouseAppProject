import requests

#product = { "description": "Sour cream", "price": '-1.5' }

#r = requests.post("http://localhost:5000/products", json=product)

message = {"username":"Joel", "password":"x200367"}
r = requests.post("http://localhost:5000/login", json=message)
print(f'Response code: {r.status_code}\n{r.text}')
message1 = {"uuid": r.text["session"], "section":"AA-AA"}
r = requests.post("http://localhost:5000/orderList", json=message1)
print(r)
