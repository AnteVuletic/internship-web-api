import React from "react";
import { Redirect, Route, Switch } from "react-router";
import HomeScreen from "./screens/HomeScreen";
import LoginScreen from "./screens/LoginScreen";
import RegisterScreen from "./screens/RegisterScreen";
import UserProvider from "./services/providers/UserProvider";

const App = () => {
  return (
    <Switch>
      <Route exact path="/login">
        <LoginScreen />
      </Route>
      <Route exact path="/register">
        <RegisterScreen />
      </Route>
      <Route path="/home">
        <UserProvider>
          <HomeScreen />
        </UserProvider>
      </Route>
      <Redirect to="/home" />
    </Switch>
  );
};

export default App;
