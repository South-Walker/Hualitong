#!/bin/sh
message=$(/usr/local/etc/GetMessage.sh $@)
/usr/local/etc/SendWechatMessage.sh $message
/usr/local/etc/SendShortMessage.sh $message
