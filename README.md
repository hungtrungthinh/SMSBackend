# SMSBackend
Backend system in SMS Solution built using .Net 4.5,ServiceStack,MySQL

#How To Deploy

In IIS 
1)  Right Click on the Sites. Click Add New Site
2) Type the name of the web site. In my case, it is "SMSBackendWeb". Type the location of the web application or 
site & provide phsical directory of application code 
3) Change associated application pool's framework to 4.0 if not.
4) Give permissions for Network Service user account & click browse 



#Settings
CHange the connection string to MySQL DB in Global.asax (connection string can be made in web.config)



