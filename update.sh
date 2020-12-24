echo "UPDATE START"
echo "GIT PULL"
git pull
echo "ASSET WEB STOP"
sudo service kestrel-wam stop
echo "PROJECT PUBLISH"
dotnet publish --configuration release
echo "ASSET WEB START"
sudo service kestrel-wam start
