import axios from "axios";
import { ChangeEvent, FormEvent, useState } from "react";

const SignInPage = () => {
  const [showPassword, setShowPassword] = useState(false);

  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const [errorMessage, setErrorMessage] = useState<string>("");

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (userName == null || email == null || password == null) {
      setErrorMessage(
        `${
          userName == null
            ? "UserName "
            : "" + email == null
            ? "Email "
            : "" + password == null
            ? "Password "
            : ""
        } bos ola bilmez`
      );
      return;
    }

    try {
      const dto = {
        UserName: userName,
        Email: email,
        Password: password,
      };
      const response = await axios.post(
        `http://localhost:5001/auth/signin`,
        dto,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.dir(response);

      if (response.data == "not found") {
        setErrorMessage("User Not Found");
        return;
      }

      const mp3TokenObj = {
        Token: response.data.token,
        UserId: response.data.userId,
        Date: new Date(response.data.expiration),
        UserName: userName,
        Email: email,
        LikeLimit: 10,
      };

      localStorage.setItem("mp3TokenObj", JSON.stringify(mp3TokenObj));

      document.location.href = "/";
    } catch (error) {
      console.error("Error during check", error);
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card p-4 shadow" style={{ width: "400px" }}>
        <h3 className="text-center mb-4">Sign In</h3>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="userName" className="form-label">
              Username
            </label>
            <input
              name="userName"
              id="userName"
              placeholder="Enter Username"
              type="text"
              className="form-control"
              required
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setUserName(e.target.value);
              }}
            />
          </div>

          <div className="mb-3">
            <label htmlFor="Email" className="form-label">
              Email
            </label>
            <input
              name="Email"
              id="Email"
              placeholder="Enter Email"
              type="email"
              className="form-control"
              required
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setEmail(e.target.value);
              }}
            />
          </div>

          <div className="mb-3 position-relative">
            <label htmlFor="password" className="form-label">
              Password
            </label>
            <input
              name="password"
              id="password"
              placeholder="Enter Password"
              type={showPassword ? "text" : "password"}
              className="form-control"
              required
              onChange={(e: ChangeEvent<HTMLInputElement>) => {
                setPassword(e.target.value);
              }}
            />
            <button
              type="button"
              onClick={togglePasswordVisibility}
              className="btn position-absolute top-50 end-0 translate-middle-y"
              style={{
                border: "none",
                position: "relative",
                bottom: "35px",
                fontSize: 18,
              }}
            >
              {showPassword ? "**" : "👁️"}
            </button>
          </div>

          {errorMessage == "" ? (
            <></>
          ) : (
            <p style={{ color: "red" }}>{errorMessage}</p>
          )}

          <button type="submit" className="btn btn-primary w-100">
            Sign In
          </button>
        </form>
        <div className="mt-3 text-center">
          <small>
            Don't have an account? <a href="/signup">Sign up</a>
          </small>
        </div>
      </div>
    </div>
  );
};

export default SignInPage;
