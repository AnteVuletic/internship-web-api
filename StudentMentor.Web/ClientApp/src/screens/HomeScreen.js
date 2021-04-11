import {
  Button,
  Container,
  Grid,
  LinearProgress,
  Typography,
} from "@material-ui/core";
import { Person, PowerSettingsNew } from "@material-ui/icons";
import axios from "axios";
import React, { useContext, useEffect, useState } from "react";
import { Route, Switch, useHistory } from "react-router";
import { UserContext } from "../services/providers/UserProvider";
import AdminScreen from "./AuthorizedScreen/AdminScreen";
import MentorScreen from "./AuthorizedScreen/MentorScreen";
import StudentScreen from "./AuthorizedScreen/StudentScreen";

const HomeScreen = () => {
  const history = useHistory();
  const {
    state: { role },
    logOut,
  } = useContext(UserContext);
  const [userInfo, setUserInfo] = useState({});

  useEffect(() => {
    if (window.location.pathname === "/home" && role) {
      history.push(`/home/${role.toLowerCase()}`);
    }
    axios.get("api/Account").then(({ data }) => setUserInfo(data));
  }, [role, history]);

  if (!role) {
    return <LinearProgress />;
  }

  return (
    <Container>
      <Grid container justify="space-between" alignItems="center">
        <Grid item>
          <Typography variant="h5">
            {userInfo.firstName} {userInfo.lastName}
          </Typography>
        </Grid>
        <Grid item>
          <Grid container justify="flex-end" spacing={1}>
            <Grid item>
              <Button
                color="primary"
                variant="contained"
                startIcon={<Person />}
              >
                Profile
              </Button>
            </Grid>
            <Grid item>
              <Button
                color="secondary"
                variant="contained"
                startIcon={<PowerSettingsNew />}
                onClick={logOut}
              >
                Log out
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Grid>

      <Switch>
        <Route path="/home/student">
          <StudentScreen />
        </Route>
        <Route path="/home/mentor">
          <MentorScreen />
        </Route>
        <Route path="/home/admin">
          <AdminScreen />
        </Route>
      </Switch>
    </Container>
  );
};

export default HomeScreen;
