create schema hualitongdb
create table hualitongdb.pwds
(
    pwds_id int identity(0,1) ,
    student_num varchar(10),
    urp_pwd varchar(11),
    jw_pwd varchar(11),
    ty_pwd varchar(11),
    PRIMARY KEY (pwds_id)
);
create table hualitongdb.users
(
    wechat_id varchar(40),
    follow_date datetime not null DEFAULT getdate(),
    pwds_id int DEFAULT '0',
    PRIMARY KEY (wechat_id)
);
create table hualitongdb.names
(
    student_num varchar(10),
    student_name varchar(10),
    PRIMARY KEY (student_num)
)
insert into hualitongdb.pwds values (null,null,null,null);


create view hualitongdb.view_wechatpwds as
select hualitongdb.users.wechat_id,hualitongdb.pwds.student_num,hualitongdb.pwds.urp_pwd,hualitongdb.pwds.ty_pwd,hualitongdb.pwds.jw_pwd
 from hualitongdb.pwds right join hualitongdb.users 
 on hualitongdb.pwds.pwds_id = hualitongdb.users.pwds_id;

CREATE TRIGGER TRIGG_PWDUPDATE
ON hualitongdb.view_wechatpwds
INSTEAD OF update 
as 
begin
declare @wechat_id varchar(40),@student_num varchar(10),@pwds_id int,
@urp_pwd varchar(11),@ty_pwd varchar(11),@jw_pwd varchar(11);
select @wechat_id = wechat_id from [inserted];
select @student_num = student_num from [inserted];
select @urp_pwd = urp_pwd from [inserted];
select @ty_pwd = ty_pwd from [inserted];
select @jw_pwd = jw_pwd from [inserted];
select @pwds_id = pwds_id from users where @wechat_id = wechat_id;

if @pwds_id = 0
    begin
        insert into hualitongdb.pwds values
        (@student_num,null,null,null)
        select @pwds_id = Max(pwds_id) from pwds
        update hualitongdb.users
        set pwds_id = @pwds_id where wechat_id = @wechat_id
    end

if @student_num is null
    begin
        select student_num = @student_num from users where wechat_id = @wechat_id;
    end
else
    begin
        update hualitongdb.pwds
        set student_num = @student_num
        where pwds_id = @pwds_id
    end
if @urp_pwd is not null
    begin
        update hualitongdb.pwds
        set urp_pwd = @urp_pwd
        where pwds_id = @pwds_id
    end
if @ty_pwd is not null
    begin
        update hualitongdb.pwds
        set ty_pwd = @ty_pwd
        where pwds_id = @pwds_id
    end
if @jw_pwd is not null
    begin
        update hualitongdb.pwds
        set jw_pwd = @jw_pwd
        where pwds_id = @pwds_id
    end
end;


 insert into hualitongdb.users (wechat_id) values ('o3dl2wZ3YisQO8GW_bd_c-QOWGsQ');
 insert into hualitongdb.users (wechat_id) values ('o3dl2wZHdvmo1sxQaiKefLRcyr_o');
 insert into hualitongdb.users (wechat_id) values ('o3dl2wUzmzcr7ZvZ6v7vi_I4Hffw');


update hualitongdb.view_wechatpwds 
set
 student_num = '10150112',
 ty_pwd = 'ty961016'
where wechat_id = 'o3dl2wZ3YisQO8GW_bd_c-QOWGsQ'