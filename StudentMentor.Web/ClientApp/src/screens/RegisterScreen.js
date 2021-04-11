import React, { useState } from "react";
import {
  Box,
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@material-ui/core";
import { Alert } from "@material-ui/lab";
import { Controller, useForm } from "react-hook-form";
import FormControlSpacing from "../components/styled/FormControlSpacing";
import { Link, useHistory } from "react-router-dom";
import axios from "axios";
import { parseJwt } from "../services/providers/UserProvider";

const RegisterScreen = () => {
  const history = useHistory();
  const {
    register,
    handleSubmit,
    watch,
    control,
    formState: { errors },
  } = useForm({
    shouldFocusError: true,
    mode: "onChange",
    defaultValues: {
      email: "",
      firstName: "",
      lastName: "",
      password: "",
      repeatPassword: "",
    },
  });
  const [backendMessage, setBackendMessage] = useState(null);

  const submitRegistration = (value) =>
    axios.post("api/Account/RegisterStudent", value).then(
      (res) => {
        localStorage.setItem("token", res.data);
        const parsedToken = parseJwt(res.data);
        history.push(`/home/${parsedToken.role.toLowerCase()}`);
      },
      ({ response }) => {
        setBackendMessage(response.data);
      }
    );

  const { ref: refRepeatPassword, ...rest } = register("repeatPassword", {
    validate: (value) =>
      value === watch("password") || "The passwords do not match",
  });

  return (
    <Container component="main" maxWidth="xs">
      <Typography component="h1" variant="h5">
        Register
      </Typography>
      <form onSubmit={handleSubmit(submitRegistration)}>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="email"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors.email ? errors.email.message : null}
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
            name="firstName"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors.firstName ? errors.firstName.message : null}
                error={!!errors.firstName}
                id="firstName"
                label="First name"
                variant="outlined"
              />
            )}
            control={control}
            rules={{
              required: "Required",
              minLength: 3,
            }}
          />
        </FormControlSpacing>
        <FormControlSpacing fullWidth variant="outlined">
          <Controller
            name="lastName"
            render={({ field: { onChange, onBlur, value } }) => (
              <TextField
                onChange={onChange}
                onBlur={onBlur}
                value={value}
                helperText={errors.lastName ? errors.lastName.message : null}
                error={!!errors.lastName}
                id="lastName"
                label="Last name"
                variant="outlined"
              />
            )}
            control={control}
            rules={{
              required: "Required",
              minLength: 3,
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
              minLength: {
                value: 4,
                message: "Password must be at least 4 characters long",
              },
            }}
          />
        </FormControlSpacing>
        <FormControlSpacing fullWidth variant="outlined">
          <TextField
            {...rest}
            inputRef={refRepeatPassword}
            helperText={
              errors?.repeatPassword ? errors.repeatPassword.message : null
            }
            error={!!errors.repeatPassword}
            variant="outlined"
            label="Repeat password"
            type="password"
            id="repeatPassword"
            autoComplete="current-password"
          />
        </FormControlSpacing>
        <Box mt={2}></Box>
        {backendMessage && <Alert severity="error">{backendMessage}</Alert>}
        <Button type="submit" fullWidth variant="contained" color="primary">
          Register
        </Button>
        <Grid container>
          <Grid item>
            <Link to="/login" variant="body2">
              Already have a account? Log in
            </Link>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default RegisterScreen;
