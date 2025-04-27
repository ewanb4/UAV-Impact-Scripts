clear;
close all;
clc;

filePath = ''; %Change this to the relevant file path noted in the console - This is for acceleration data
collisionFilePath = ''; %Change this to the relevant file path noted in the console - This os for collision data 

% Reads Data from files and converts to tables
data = readtable(filePath);
collisionData = readtable(collisionFilePath);

% Collects data from the tables
timeOfCollision = table2array(collisionData(:,1));
relativeVelocity = table2array(collisionData(:,4));
forceOfCollision= table2array(collisionData(:,3));
momentExperienced = table2array(collisionData(:,5)) .* forceOfCollision;

timeStamp = table2array(data(:,1));
angularAccelerationY = table2array(data(:,4));
angularAccelerationX = string(table2array(data(:,3)));
angularAccelerationZ = string(table2array(data(:,5)));
angularVelocityX = string(table2array(data(:,9)));
angularVelocityY = table2array(data(:,10));
angularVelocityZ = string(table2array(data(:,11)));
angularJerkX = string(table2array(data(:,6)));
angularJerkY = table2array(data(:,7));
angularJerkZ = string(table2array(data(:,8)));

accelerationX = string(table2array(data(:,12)));
accelerationY = table2array(data(:,13));
accelerationZ = string(table2array(data(:,14)));

jerkX = string(table2array(data(:,15)));
jerkY = table2array(data(:,16));
jerkZ = string(table2array(data(:,17)));

velocityX = string(table2array(data(:,18)));
velocityY = table2array(data(:,19));
velocityZ = string(table2array(data(:,20)));

xRotation = table2array(data(:,21));
yRotation = table2array(data(:,22));
zRotation = table2array(data(:,23));

% Converts the data in tables to integers to be plotted
for i = 1:length(angularAccelerationY)
    tempAngAccelX = split(angularAccelerationX(i),'(',2);
    XAngAccelStringValue = tempAngAccelX(2);
    angularAccelerationXFormatted(i) = str2double(XAngAccelStringValue);

    tempAccelX = split(accelerationX(i),'(',2);
    XAccelStringValue = tempAccelX(2);
    accelerationXFormatted(i) = str2double(XAccelStringValue);

    tempAccelZ = split(accelerationZ(i),')',2);
    ZAccelStringValue = tempAccelZ(1);
    accelerationZFormatted(i) = str2double(ZAccelStringValue);

    tempAngAccelZ = split(angularAccelerationZ(i),')',2);
    ZAngAccelStringValue = tempAngAccelZ(1);
    angularAccelerationZFormatted(i) = str2double(ZAngAccelStringValue);
    
    tempAngJerkX = split(angularJerkX(i), '(',2);
    XAngJerkStringValue = tempAngJerkX(2);
    angularJerkXFormatted(i) = str2double(XAngJerkStringValue);

    tempAngJerkZ = split(angularJerkZ(i), ')',2);
    ZAngJerkStringValue = tempAngJerkZ(1);
    angularJerkZFormatted(i) = str2double(ZAngJerkStringValue);

    tempJerkX = split(jerkX(i), '(',2);
    XJerkStringValue = tempJerkX(2);
    JerkXFormatted(i) = str2double(XJerkStringValue);

    tempJerkZ = split(jerkZ(i), ')',2);
    ZJerkStringValue = tempJerkZ(1);
    JerkZFormatted(i) = str2double(ZJerkStringValue);

    tempAngVelX = split(angularVelocityX(i),'(',2);
    XAngVelStringValue = tempAngVelX(2);
    angularVelocityXFormatted(i) = str2double(XAngVelStringValue);

    tempAngVelZ = split(angularVelocityZ(i),')',2);
    ZAngVelStringValue = tempAngVelZ(1);
    angularVelocityZFormatted(i) = str2double(ZAngVelStringValue);

    tempVelX = split(velocityX(i),'(',2);
    XVelStringValue = tempVelX(2);
    velocityXFormatted(i) = str2double(XVelStringValue);

    tempVelZ = split(velocityZ(i),'(',2);
    ZVelStringValue = tempVelX(1);
    velocityZFormatted(i) = str2double(ZVelStringValue);    

    angularAccelerationTotal(i) = sqrt(angularAccelerationXFormatted(i)^2 + angularAccelerationZFormatted(i)^2 + angularAccelerationY(i)^2);
    accelerationTotal(i) = sqrt(accelerationZFormatted(i)^2 + accelerationY(i)^2 + accelerationXFormatted(i)^2);

    angularJerkTotal(i) = sqrt(angularJerkXFormatted(i)^2 + angularJerkY(i)^2 + angularJerkZFormatted(i)^2);
    JerkTotal(i) = sqrt(JerkXFormatted(i)^2 + jerkY(i)^2 + JerkZFormatted(i)^2);

    angulatVelocityTotal(i) = sqrt(angularVelocityZFormatted(i)^2 + angularVelocityXFormatted(i)^2 + angularVelocityY(i)^2);
    velocityTotal(i) = sqrt(velocityZFormatted(i)^2 + velocityY(i)^2 + velocityZFormatted(i)^2);


end
maxAngAccel = max(angularAccelerationTotal);

% Calculates Head Injury Criterion
HIC = (0.036)*((1/0.036) .* (accelerationTotal./9.81) .* 0.036).^2.5; 

% Creates Strings for file names when exporting
ragdollTitle = split(filePath,'\',2);
ragdollTitle = string(ragdollTitle(8));
stringAngJerkRagdoll = sprintf('%s AngularJerkGraph.jpg',ragdollTitle);
stringAngAccelerationRagdoll = sprintf('%s AngularAccelerationGraph.jpg',ragdollTitle);
stringAngJerkAxesRagdoll = sprintf('%s AngularJerkGraphAxes.jpg',ragdollTitle);
stringAngAccelerationAxesRagdoll = sprintf('%s AngularAccelerationGraphAxes.jpg',ragdollTitle);
stringAngVel = sprintf('%s AngularVelocityGraph.jpg',ragdollTitle);
stringAngVelAxes = sprintf('%s AngularVelocityGraphAxes.jpg',ragdollTitle);
stringHIC = sprintf('%s HICGraph.jpg',ragdollTitle);
stringPosition = sprintf('%s PositionOffset.jpg',ragdollTitle);
stringJerkRagdoll = sprintf('%s JerkGraph.jpg',ragdollTitle);
stringAccelerationRagdoll = sprintf('%s AccelerationGraph.jpg',ragdollTitle);
stringJerkAxesRagdoll = sprintf('%s JerkGraphAxes.jpg',ragdollTitle);
stringAccelerationAxesRagdoll = sprintf('%s AccelerationGraphAxes.jpg',ragdollTitle);


HICTitle = sprintf('Impact moment: %f Nm',momentExperienced(1));

figure(1)
plot(timeStamp,angularAccelerationXFormatted,"LineWidth",4)
%title("Relative Acceleration of the Ragdolls Head vs Simulation Time")
hold on
plot(timeStamp,angularAccelerationY,"LineWidth",4)
plot(timeStamp,angularAccelerationZFormatted,"LineWidth",4)
xline(timeOfCollision(1),"r--",LineWidth=2);
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Angular Acceleration (rad/s^2)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
legend("X-axis","Y-axis","Z-axis",'Location','NorthEast')
grid on
set(gcf,'color','white')
f = gcf;
exportgraphics(f,stringAngAccelerationAxesRagdoll,'Resolution',400)
hold off

figure(2)
plot(timeStamp,angularJerkXFormatted,"LineWidth",4)
%title("Relative Jerk of the Ragdolls Head vs Simulation Time","FontSize",20)
hold on
plot(timeStamp,angularJerkY,"LineWidth",4)
plot(timeStamp,angularJerkZFormatted,"LineWidth",4)
set(gca,"FontSize",20)
xlabel("Simulation Time (s)","FontSize",20)
ylabel("Angular Jerk (rad/s^3)","FontSize",20)
xline(timeOfCollision(1),"r--",LineWidth=2);
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7]);
legend("X-axis","Y-axis","Z-axis","Fontsize",20,"Location","NorthEast")
grid on
f = gcf;
set(gcf,'color','white')
exportgraphics(f,stringAngJerkAxesRagdoll,'Resolution',400)
hold off

figure(3)
plot(timeStamp,angularAccelerationTotal,"LineWidth",4)
%title("Relative Acceleration of the Ragdolls Head vs Simulation Time")
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Angular Acceleration (rad/s^2)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
xline(timeOfCollision(1),"r--",LineWidth=2);
grid on
set(gcf,'color','white')
f = gcf;
exportgraphics(f,stringAngAccelerationRagdoll,'Resolution',400)
hold off

figure(4)
plot(timeStamp,angularJerkTotal,"LineWidth",4)
%title("Relative Jerk of the Ragdolls Head vs Simulation Time")
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Angular Jerk (rad/s^3)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
grid on
set(gcf,'color','white')
xline(timeOfCollision(1),"r--",LineWidth=2);
f = gcf;
exportgraphics(f,stringAngJerkRagdoll,'Resolution',400)
hold off

figure(5)
plot(timeStamp,HIC,"LineWidth",4)
hold on
set(gca,"FontSize",20)
xline(timeOfCollision,"r--",LineWidth=2);
xlabel("Simulation Time (s)")
ylabel("HIC value")
grid on
set(gcf, 'Color','white')
title(HICTitle)
f = gcf;
ylim([0,max(HIC) + 10])
exportgraphics(f,stringHIC,'Resolution',400)
hold off

figure(6)
plot(timeStamp,angulatVelocityTotal,"LineWidth",4)
f = gcf;
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Angular Velocity (rad/s)")
ylim([0 max(angulatVelocityTotal)+10])
grid on
set(gcf,'color','white')
xline(timeOfCollision(1),"r--",LineWidth=2);
exportgraphics(f,stringAngVel,'Resolution',400)
hold off

figure(7)
plot(timeStamp, angularVelocityXFormatted,"LineWidth",4)
f = gcf;
hold on
plot(timeStamp,angularVelocityY,"LineWidth",4)
plot(timeStamp, angularVelocityZFormatted,"LineWidth",4)
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Angular Velocity (rad/s)")
xline(timeOfCollision(1),"r--",LineWidth=2);
legend("X-axis","Y-axis","Z-axis","Fontsize",20,"Location","NorthEast")
grid on
set(gcf,'color','white')
exportgraphics(f,stringAngVelAxes,'Resolution',400)
hold off

figure(8)
plot(timeStamp, xRotation,"LineWidth",4)
hold on
plot(timeStamp, yRotation,"LineWidth",4)
plot(timeStamp, zRotation,"LineWidth",4)
f = gcf;
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Position offset (degrees)")
xline(timeOfCollision(1),"r--",LineWidth=2);
legend("X-axis","Y-axis","Z-axis","Fontsize",20,"Location","NorthEast")
grid on
set(gcf,'color','white')
exportgraphics(f,stringPosition,'Resolution',400)
hold off

figure(9)
plot(timeStamp,accelerationXFormatted,"LineWidth",4)
%title("Relative Acceleration of the Ragdolls Head vs Simulation Time")
hold on
plot(timeStamp,accelerationY,"LineWidth",4)
plot(timeStamp,accelerationZFormatted,"LineWidth",4)
xline(timeOfCollision(1),"r--",LineWidth=2);
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Acceleration (m/s^2)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
legend("X-axis","Y-axis","Z-axis",'Location','NorthEast')
grid on
set(gcf,'color','white')
f = gcf;
exportgraphics(f,stringAccelerationAxesRagdoll,'Resolution',400)
hold off

figure(10)
plot(timeStamp,JerkXFormatted,"LineWidth",4)
%title("Relative Jerk of the Ragdolls Head vs Simulation Time","FontSize",20)
hold on
plot(timeStamp,jerkY,"LineWidth",4)
plot(timeStamp,JerkZFormatted,"LineWidth",4)
set(gca,"FontSize",20)
xlabel("Simulation Time (s)","FontSize",20)
ylabel("Jerk (m/s^3)","FontSize",20)
xline(timeOfCollision(1),"r--",LineWidth=2);
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7]);
legend("X-axis","Y-axis","Z-axis","Fontsize",20,"Location","SouthEast")
grid on
set(gcf,'color','white')
exportgraphics(f,stringJerkAxesRagdoll,'Resolution',400)
hold off

figure(11)
plot(timeStamp,accelerationTotal,"LineWidth",4)
%title("Relative Acceleration of the Ragdolls Head vs Simulation Time")
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Acceleration (m/s^2)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
xline(timeOfCollision(1),"r--",LineWidth=2);
grid on
set(gcf,'color','white')
f = gcf;
exportgraphics(f,stringAccelerationRagdoll,'Resolution',400)
hold off

figure(12)
plot(timeStamp,JerkTotal,"LineWidth",4)
%title("Relative Jerk of the Ragdolls Head vs Simulation Time")
set(gca,"FontSize",20)
xlabel("Simulation Time (s)")
ylabel("Jerk (m/s^3)")
xlim([timeOfCollision(1) - 0.3,timeOfCollision(1) + 0.7])
grid on
set(gcf,'color','white')
xline(timeOfCollision(1),"r--",LineWidth=2);
f = gcf;
exportgraphics(f,stringJerkRagdoll,'Resolution',400)
hold off