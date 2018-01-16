Login: admin@admin.com
Password: Admin123123

Getting REST API bearer token:

POST /umbraco/oauth/token HTTP/1.1
Host: localhost:59327
Cache-Control: no-cache

grant_type=password&username=admin@admin.com&password=Admin123123


Getting item:

GET /umbraco/rest/v1/content/1063 HTTP/1.1
Host: localhost:59327
Authorization: Bearer <your_token>
Cache-Control: no-cache


Getting published item:

GET /umbraco/rest/v1/content/published/1063 HTTP/1.1
Host: localhost:59327
Authorization: Bearer <your_token>
Cache-Control: no-cache


