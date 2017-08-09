#!/bin/sh
result=""
for args in $@
do
        result="${result}""${args}"
done
echo $result
