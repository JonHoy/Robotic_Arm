NET.addAssembly([pwd '\Robot_Arm.GPU.dll'])
Colors = uint8([255 0 0 0; 0 255 0 0; 0 0 255 0; 0 0 0 0; 255 255 255 0; 255 255 0 0; 0 255 255 0]);
Colors = [Colors; Colors; Colors; Colors; Colors];
SelectionIndex = 0:(numel(Colors)/4 - 1);
SelectedVal = 1;
DataOriginal = imread('LeftColor.jpg');
Data = DataOriginal;
Data(:,:,4) = zeros(720,1280,'uint8');
for i = 1:100
    tic;Blobs = Robot_Arm.GPU.SegmentColors(Data, Colors, SelectionIndex, SelectedVal);toc
    disp(Blobs)
end
