docker run -d --rm --name mongo -p 27117:27017 \
-v mongodbdata:/data/db \
-e MONGODB_ROOT_USER=mongoadmin \
-e MONGODB_ROOT_PASSWORD=Pass#word1 \
bitnami/mongodb