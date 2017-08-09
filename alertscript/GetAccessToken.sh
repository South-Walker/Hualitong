#!/bin/sh
appid="wx02d0aa4845c8f9ae"
secret="77c0f9d24fd9c63c28aadf6f05d04311"
curl -s "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=$appid&secret=$secret"|awk -F ':' '{print $2}'|awk -F '"' '{print $2}'>&1
