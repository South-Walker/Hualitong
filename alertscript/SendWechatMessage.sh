#!/bin/sh
token=$(/usr/local/etc/GetAccessToken.sh)
url=https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=$token
message=$1
tousera=oRunv0b05u9fTZ18kSmX3Ot_1N_g
touserb=oRunv0b05u9fTZ18kSmX3Ot_1N_g

posta='{"touser":"'$tousera'","msgtype":"text","text":{"content":"'$message'"}}'
postb='{"touser":"'$touserb'","msgtype":"text","text":{"content":"'$message'"}}'
curl -d "$posta" "$url"
curl -d "$postb" "$url"
