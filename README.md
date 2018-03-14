![](https://raw.github.com/South-Walker/Hualitong/master/doc/hualitongcolorful.png)
华理通 - 基于微信的WebApp
==

华理通是无偿服务于华理学子的志愿团队，本项目是由南行开发，授权华理通团队使用的基于微信的Web应用，指在为华理学子提供一个方便快捷的教务、生活信息查询平台。<br>
本项目高峰时期使用人数约1500人，公众号关注人数约2300人，部分后台数据仍有留存<br>
由于种种原因，本项目停止维护
--

### 逻辑框图

![](https://raw.github.com/South-Walker/Hualitong/master/doc/hualitong.png)

### 具体功能

* 实现了与微信后台的交互
* 实现了对华理教务处系统的模拟登陆与基于教务处用户系统的微信用户认证
* 实现了对教务处页面内容的正则分析与数据提取
* 在教务处设置登陆验证码后自写了一套验证码识别系统对验证码进行识别，成功率在99%以上
![验证码](https://raw.github.com/South-Walker/Elearn/master/doc/VerifyCode.gif)
* 在教务处对外网封闭后将具体数据缓存在服务器本地以保证功能正常运行
* 以System.Net为基础重新封装了方便记录Cookie，面向Http的MyHttpHelper类
* 以System.Drawing为基础做图，实现了由课程数据生成课程表图片的方法，具体见下
* 实现了基于阿里云短信SDK的短信通知方法，与参考[mafly/Mail](https://github.com/South-Walker/Mail)实现的邮箱通知方法

### 效果图
由于年深日久，实际运行图难以获得，以下图片是在测试阶段在私人公众号上留存的记录<br>
![](https://raw.github.com/South-Walker/Hualitong/master/doc/Grade.png)
![](https://raw.github.com/South-Walker/Hualitong/master/doc/Classtable.png)
![](https://raw.github.com/South-Walker/Hualitong/master/doc/bigClasstable.png)

### 日志
不存在的