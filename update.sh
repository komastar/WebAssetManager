echo "UPDATE START"
echo "GIT PULL"
git pull
echo "NGINX STOP"
sudo service nginx stop
echo "ORAK ASSET WEB STOP"
sudo service kestrel-orakassetweb stop
echo "PROJECT PUBLISH"
dotnet publish --configuration release
echo "ORAK ASSET WEB START"
sudo service kestrel-orakassetweb start
echo "NGINX START"
sudo service nginx start
