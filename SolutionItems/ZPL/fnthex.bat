@echo 开始注册
copy fnthex32.dll %windir%\SysWOW64\
regsvr32 %windir%\system32\fnthex32.dll /s
@echo fnthex32.dll注册成功
@pause