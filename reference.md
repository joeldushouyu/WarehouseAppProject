# general knowledge of Matlab
```matlab
t1 = [-3:1:3]; %1D matrix, from -3 to 3, with step size of 1
t2 =linespace(-5,5,7);% 1x7 Matrix, with same spacing between each of them. Note: sibling function logspace()
t1 = t1'; % inverse matrix

%Difference between matrix operation and operation by element:
t1 = t1*2; % operation on the whole matrix.
t2 = t1./2; % operation on each element, in this case divide each by 2

% Concating matrix
a = [1 3 5]
b = a.^2; % raise each element by power of 2

d = [a b]  % match by row, 1x6
e = [a ; b] % match by column,2x3
f = [a' b']% match by row, 3x2
g = [a' ; b']% match by column 6x1


% logic matrix
j = 1:2:12
bigs = j>5; % 1x12, logic matrix of either 0's or 1's
smalls = ~bigs; % example of logic operation on logic matrix
j(bigs);% only return value in matrix that is True

indices = find(bigs); % find index of non-zero values
j(indices); % same result as j(bigs);


```
# Function
```matlab
%lambda function
u = @(t) 1.0 .*(t>=0); % lambda function of u(t)

% customize function
%1. create a new file for function
% write the function,  make sure function name match the filename !!! (in this case is myfunc.m)

function [result] = myfunc(x)
% input- a digit in 'x;, output - a digit in 'result'
result = x*x+1;
end

% call the function
myfunc(6); % now the result is already stored in 'result'
```
# 2D Plotting 
```matlab
tt = [-0.3:0.1:1.7]
s1 = (1/2).^(tt); 

 % plot a discrete graph
stem(s1);
% plot a continous graph, with two curve on same plot
plot(tt, sin(tt), tt, cos(tt));
legend("sin(tt)", "cos(tt)"); %legend of graph
xlim([0 5])% xlimit
ylim([0 5])% ylimit
xlabel('omega') % xlabel
ylabel('result') % ylabel
title("Graph 1") % plot title

% Subplot
% format - subplot(total graph in column, total graph in row, graph number start with one)
% example below plot two graph vertically
subplot(2,1,1);
... plotting
subplot(2,1,2);

% 2D parametric plot
r = 10; theta = linspace(0, 2*pi, 100); 
x = r*cos(theta); y = r*sin(theta);
plot(x, y); % Plot the 2D parametric plot

```

# 3D Plotting
```matlab

% plot 3 D plots
plot3(real_value, imaginary_value, t); 

% Surface plot in 3D
% Surf Graph
[X,Y] = meshgrid(-8:0.5:8); % Both x, y axis from -8 to 8. Note: It should be a squre matrix!!
R = sqrt(X.^2 + x.^2)+eps;
Z = (sin(R)./R);
surf(X,Y,Z);  % Surf graph: Making graph smoother by making values in between data points

% Mesh Graph
mesh(X,Y,Z, 'EedgeColor;, 'black'); Mesh graph just put 3D plot base on given datapoint. Thus, this imply of missing gap.
hidden off
surf(X,Y,Z)
colormap hsv
colorbar
alpha(0.4); % Determine the transparency of the plot
```

# Image processing
```matlab
yellow = imread('picture.jpg');yellow is a 3D matrix.
image(yellow); % load image in matlab

% (x,y,1) - red value ; (x,y,2) - green value; (x,y,3) - blue value

% example of filter
filter([1/6, 1/6, 1/6,1/6, 1/6, 1/6],1,yellow); % average of 6 nearest pixel in verticall
conv(h1, h2, A); % apply h1 to rows and h2 to columns of Matrix A. Need to do it 3 times for each Red, Green, Blue matrix.

% example of applying mask and replace on image
%1. mask3d, matrix determine 
yellowShortenSize = size(yellowShorten);
mask3d = zeros(yellowShortenSize(1), yellowShortenSize(2),yellowShortenSize(3));
mask3d(:,:,1) = mask;
mask3d(:,:,2) = mask;
mask3d(:,:,3) = mask;

% example of apply filters on image
mask3d = logical(mask3d);
result(mask3d) = yellowShorten(mask3d);
```


# Continous Signal
```Matlab

%Convoluation
conv(x, h); %Both x, h is a 1D matrix.

%Transfer function
s = tf('s');  making as a transfer function symbol
H1 = (20*s + 80)/ ((s+3)*(s+10))

zpk(H1) ; the refactor of H1
[z,p,k] = zpkdata(H1);  getting the zeros, poles, gains of the function

% Note: the output of z, p,k is cell (similar idea to tuple)
% cell example
frog = {[1 2], 'hello'}
frog{1}

% plotting out the transfer function
%   Plot in 2D
bode(H1);
pzmap(H1); % poles and zeros
impulse(H1); % impulse response
stepplot(H1); % response of u(t)

%   Plot in 3D 
[X,Y] = meshgrid(-5:0.1:5);
s = X + Y*j;
Z = abs( (20*s + 80) ./ ((s+3).*(s+10))); %Need take absoute value, since the Laplace transform is a 4D plot.
surf(X,Y,Z);


%Symbolic function
x = sym('x');
y = x^2 + 2;

diff(y); derivative
int(y); integral
H4 = laplace(y); Note, this is just a string, not going to be understand by matlab
H5 = sym2tf(laplace(y)) ; doing so will make it understand. See package sym2tf
```

# Discrete Signal
``` matlab

% Fourier transform
n2 = [0:1:63]; % take 63 samples
x = cos(n2); 
X = fft(x);  % The fourier transform
X = fftshift(X); % shift to the center

Fs = 31.25;  %sample frequency (sample/second)
stepSize = (1 ./ (64*(1/Fs)))
tt = (1/Fs) .* n2; % (32*10^-3)  is (second/sample)
ff = [ -32*stepSize : stepSize: 31*stepSize]; % the frequency spectrum

stem(tt, x2); % time plot
stem(ff, abs(X2)); % magnitude spectrum


% Z transform
syms z n  % symbolic equation in term of z
H = (z.^2);

h = iztrans(H,z,n); %Inverse Z transform in terms of n
h = simplify(h);

% plotting it out
[s,w] = meshgrid(-1:0.5:1);
z = s+j*w;
H1 = abs(z.^2 / (z.^2 - Z));  %note: need the absolute value, since the result is a 4 D result

surf(s,w,H1);
mesh(s,w,H1)
```
