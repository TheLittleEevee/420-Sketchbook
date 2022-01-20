ArrayList<PVector> blox = new ArrayList<PVector>();

float threshold = 0.5;
float zoom = 10;

float sizeOfBlocks = 10;
int dimOfBlocks = 30;

void setup(){
  size(800, 500, P3D);
  noStroke();
  generateTerrainData();
}

void generateTerrainData(){
  //Get rid of any existing vectors
  blox.clear();
  
  //Clamp values
  zoom = constrain(zoom, 1, 50);
  
  //Make an array to hold the density data
  float[][][] data = new float[dimOfBlocks][dimOfBlocks][dimOfBlocks];
  
  //Set data to Perlin Noise using zoomed pos as an input
  for(int x = 0; x < dimOfBlocks; x++){
    for(int y = 0; y < dimOfBlocks; y++){
      for(int z = 0; z < dimOfBlocks; z++){
        data[x][y][z] = noise(x/zoom, y/zoom, z/zoom) + y / 100.0;
      }
    }
  }
  
  //Check for complete occlusion (if it's completely surrounded by cubes)
  // ...
  
  //Spawn blocks where density > threshold
  for(int x = 0; x < dimOfBlocks; x++){
    for(int y = 0; y < dimOfBlocks; y++){
      for(int z = 0; z < dimOfBlocks; z++){
        if(data[x][y][z] > threshold){
          blox.add(new PVector(x, y, z));
        }
      }
    }
  }
}

void checkInput(){
  boolean shouldRegen = false;
  
  if(Keys.PLUS()){
    threshold += .01;
    shouldRegen = true;
  }
  if(Keys.MINUS()){
    threshold -= .01;
    shouldRegen = true;
  }
  
  if(Keys.BRACKET_LEFT()){
    zoom += .1;
    shouldRegen = true;
  }
  if(Keys.BRACKET_RIGHT()){
    zoom -= .1;
    shouldRegen = true;
  }
  
  if(shouldRegen) generateTerrainData();
}

void draw(){
  checkInput();
  background(0);
  lights();
  
  pushMatrix();
  
  //Reposition the camera from the corner of the window to the center
  translate(width/2, height/2);
  //Rotate the camera controlled by the mouse
  rotateX(map(mouseY, 0, width, -1, 1));
  rotateY(map(mouseX, 0, width, -PI, PI));
  //Reposition the camera by finding the offset from the center of the cube to the corner
  float d = -dimOfBlocks * sizeOfBlocks / 2;
  translate(d, d, d);
    
  //Render each cube
  for(PVector pos : blox){ //For each pos that exists within blox
    pushMatrix();
    //Move the origin to the position
    translate(pos.x * sizeOfBlocks, pos.y * sizeOfBlocks, pos.z * sizeOfBlocks);
    //Render a cube
    box(sizeOfBlocks, sizeOfBlocks, sizeOfBlocks);
    popMatrix();
  }
  
  popMatrix();
}
