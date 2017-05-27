基于微信的WebApp
==

华理通（南行负责部分）
--

### 逻辑框图

![hualitong](http://xiaoliming96.com/images/hualitong.png)  

### 日志

#### 2017-5-26

>* 重构了代码
>>将所有封装的类单独放入models目录下，并对其他控制器添加了对应命名空间的引用
>* 添加了ReadMe文件
>>添加了对华理通项目的说明、逻辑框图与日志
>>添加了410打印店项目的说明

#### 2017-5-27
>* 重构了代码
>>将APIController下生成POST字符串的代码由拼接改为了使用占位符
>>>```c#
>>>string post = string.Format("txtuser={0}&txtpwd={1}",studentnum, pwd);
>>>```
>>>由于学校网络原因，无法测试，故注释掉原本代码

410打印店
--

### 逻辑框图
