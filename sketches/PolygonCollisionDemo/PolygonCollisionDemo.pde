Polygon gon1;
Polygon gon2;

void setup(){
  size(800, 600);
  gon1 = new Polygon(6);
  gon2 = new Polygon(3);
}

void draw(){
  //Update
  gon2.rotation += .01;
  gon2.position.x = mouseX;
  gon2.position.y = mouseY;
  
  gon2.isColliding  = gon1.doesOverlap(gon2);
  
  gon1.update();
  gon2.update();
  
  //Draw
  background(100);
  gon1.draw();
  gon2.draw();
}
