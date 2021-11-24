number = ["A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]
s = "CZ"

begin = number.index(s[0])*260 + number.index(s[1])*10+1
end = begin+9

print(begin, end)