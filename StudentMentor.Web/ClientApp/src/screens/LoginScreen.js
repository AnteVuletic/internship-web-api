import {
  Box,
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import { Alert } from "@material-ui/lab";
import axios from "axios";
import React, { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Link, useHistory } from "react-router-dom";
import FormControlSpacing from "../components/styled/FormControlSpacing";
import { parseJwt } from "../services/providers/UserProvider";

const LoginScreen = () => {
  const history = useHistory();
  const {
    handleSubmit,
    control,
    formState: { errors },
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      password: "",
      email: "",
    },
  });

  const [backendMessage, setBackendMessage] = useState(null);

  const handleLogin = (value) =>
    axios.post("api/Account/Login", value).then(
      (res) => {
        localStorage.setItem("token", res.data);
        const parsedToken = parseJwt(res.data);
        history.push(`/home/${parsedToken.role.toLowerCase()}`);
      },
      ({ response }) => setBackendMessage(response.data)
    );

  return (
    <Container component="main" maxWidth="xs">
      <Typography component="h1" variant="h5">
        Log in
      </Typography>
      <form onSubmit={handleSubmit(handleLogin)}>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="email"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors?.email ? errors.email.message : null}
                error={!!errors.email}
                id="email"
                label="Email"
                variant="outlined"
              />
            )}
            control={control}
            rules={{
              required: "Required",
              pattern: {
                value: /^\S+@\S+\.\S+$/,
                message: "Invalid email format",
              },
            }}
          />
        </FormControlSpacing>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="password"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors?.password ? errors.password.message : null}
                error={!!errors.password}
                variant="outlined"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
              />
            )}
            control={control}
            rules={{
              required: "Required",
            }}
          />
        </FormControlSpacing>
        <Box mt={2}></Box>
        {backendMessage && <Alert severity="error">{backendMessage}</Alert>}
        <Button type="submit" fullWidth variant="contained" color="primary">
          Log in
        </Button>
        <Grid container>
          <Grid item>
            <Link to="/register" variant="body2">
              Don't have an account? Register
            </Link>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default LoginScreen;
