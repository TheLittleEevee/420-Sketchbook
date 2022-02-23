class FlowfieldGrid{
  //Properties
  int res = 10;
  float zoom = 10;
  boolean isHidden = false;
  
  //Current/Cached data
  float[][] data;
  
  FlowfieldGrid(){
    build();
  }
  
  void build(){
    data = new float[res][res];
    
    int thresh = 3;
    float w = getCellWidth();
    float h = getCellHeight();
    
    for (int x = 0; x < data.length; x++){
      for (int y = 0; y < data[x].length; y++){
        float val = noise(x/zoom, y/zoom);
        val = map(val, 0, 1, -PI * 2, PI * 2);
        
        //Detect if cell is near side of screen
        //If it is, use atan2() to find angle to center of screen
        if (x < thresh || y < thresh || x >= data.length - thresh || y >= data[0].length - thresh){
          float dy = (height / (float)2) - (y * h + h/2);
          float dx = (width / (float)2) - (x * w + w/2);
          val = atan2(dy, dx);
        }
        
        
        data[x][y] = val;
      }
    }
  }
  
  void update(){
    boolean rebuild = false;
    
    if (Keys.onDown(32)){
      isHidden = !isHidden;
      rebuild = true;
    }
    
    if (Keys.onDown(37)){
      res--;
      rebuild = true;
    }
  
    if (Keys.onDown(39)){
      res++;
      rebuild = true;
    }
    
    if (Keys.onDown(38)){
      zoom += 1;
      rebuild = true;
    }
    
    if (Keys.onDown(40)){
      zoom -= 1;
      rebuild = true;
    }
    
    res = constrain(res, 4, 50);
    zoom = constrain(zoom, 5, 50);
    if (rebuild) build();
  }
  
  void draw(){
    if (isHidden) return;
    float w = getCellWidth();
    float h = getCellHeight();
    
    for (int x = 0; x < data.length; x++){
      for (int y = 0; y < data[x].length; y++){
        float val = data[x][y];
        
        float topleftX = x * w;
        float topleftY = y * h;
        pushMatrix();
        translate(topleftX + w / 2, topleftY + h / 2);
        rotate(val);
        
        float hue = map(val, -PI, PI, 0, 255);
        
        stroke(255);
        fill(hue, 255, 255);
        ellipse(0, 0, 20, 20);
        //rect(-w / 2, -h / 2, w, h); //OR
        //rectMode(CENTER);
        //rect(0, 0, w, h);
        
        line(0, 0, 25, 0);
        noStroke();
        
        popMatrix();
      } //End y loop
    } //End x loop
  } //End draw
  
  float getCellWidth(){
    return width / res;
  }
  
  float getCellHeight(){
    return height / res;
  }
  
  float getDirectionAt(PVector p){
    return getDirectionAt(p.x, p.y);
  }
  
  float getDirectionAt(float x, float y){
    int ix = (int)(x / getCellWidth());
    int iy = (int)(y / getCellHeight());
    
    if (ix < 0 || iy < 0 || ix >= data.length || iy >= data[0].length){
      //Invalid coordinate
      float dy = (height / (float)2) - y;
      float dx = (width / (float)2) - x;
      return atan2(dy, dx);
    }
    
    return data[ix][iy];
  }
}
