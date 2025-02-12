void setup(){
  size(400, 400);
  noStroke();
  colorMode(HSB);
}

void draw(){
  //float zoom = map(mouseX, 0, width, 10, 100);
  float zoom = 100;
  float time = millis()/1000.0;
  
  float threshold = mouseX / (float)width;
  
  for (int x = 0; x < width; x++){
    for (int y = 0; y < width; y++){
      float val = map(noise(x/zoom, y/zoom, time), .1, .9, 0, 1); //Value without map is from about .2 or .1 to .8 or .9 instead of 0 to 1
      
      fill(val < threshold ? 0 : 255);
      
      rect(x, y, 1, 1);
    }
  }
}
