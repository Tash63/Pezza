import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class MainPage extends StatelessWidget {
  final Color _backgroundcolor = Color.fromRGBO(220, 49, 70, 100);
  final Color _bannercolor = Color.fromRGBO(254, 200, 69, 100);
  late final double _screenwidth;
  late final double _screenheight;

  @override
  Widget build(BuildContext context) {
    _screenwidth = MediaQuery.of(context).size.width;
    _screenheight = MediaQuery.of(context).size.height;
    return Scaffold(
      body: Column(children: [
        SizedBox(
          width: _screenwidth,
          height: _screenheight * 0.102,
          child: Container(
            color: _bannercolor,
            child: LayoutBuilder(builder: (ctx, constraints) {
              return Row(
                children: [
                  Image(image: AssetImage('assets/images/pezza-logo.png')),
                  SizedBox(
                    width: constraints.maxWidth * 0.15,
                  ),
                  Text(
                    "Pezza",
                    style: TextStyle(
                        fontSize: constraints.maxHeight * 0.6,
                        color: Colors.white),
                  )
                ],
              );
            }),
          ),
        ),
        SizedBox(
          height: _screenheight * (1 - 0.102),
          child: LayoutBuilder(builder: (ctx, constriants) {
            return Column(
              children: [
                Text(
                  "Welcome",
                  style: TextStyle(color: Colors.white),
                ),
                SizedBox(
                  height: 10,
                ),
                ElevatedButton(
                  onPressed: () => {},
                  child: Text(
                    "Log-In",
                    style: TextStyle(color: Colors.white),
                  ),
                  style:
                      ElevatedButton.styleFrom(backgroundColor: _bannercolor),
                ),
                SizedBox(
                  height: 10,
                ),
                Text(
                  "Or",
                  style: TextStyle(color: Colors.white),
                ),
                SizedBox(
                  height: 10,
                ),
                SizedBox(
                  height: constriants.maxHeight*0.07,
                    width: constriants.maxWidth*0.6,
                    child: ElevatedButton(
                  onPressed: () => {},
                  child: Text(
                    "Create Account",
                    style: TextStyle(color: Colors.white),
                  ),
                  style:
                      ElevatedButton.styleFrom(backgroundColor: _bannercolor),
                ))
              ],
            );
          }),
        )
      ]),
      backgroundColor: _backgroundcolor,
    );
  }
}
