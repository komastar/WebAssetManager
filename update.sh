echo "UPDATE START"
echo "GIT PULL"
git pull
echo "NGINX STOP"
sudo service nginx stop
echo "ASSET WEB STOP"
sudo service kestrel-wam stop
echo "PROJECT PUBLISH"
dotnet publish --configuration release
echo "ASSET WEB START"
sudo service kestrel-wam start
echo "NGINX START"
sudo service nginx start
