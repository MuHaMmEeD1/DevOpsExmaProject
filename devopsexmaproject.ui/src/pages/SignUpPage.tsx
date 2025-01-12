import { useState, ChangeEvent, FormEvent } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const SignUpPage = () => {
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [showConfirmPassword, setShowConfirmPassword] =
    useState<boolean>(false);
  const [confirmEmail, setConfirmEmail] = useState<boolean>(false);

  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [emailVerifictionCode, setEmailVerificationCode] = useState<string>("");

  const [verificationCode, setVerificationCode] = useState<string>("");

  const [errorMessage, setErrorMessage] = useState<string>("");

  // Navigator
  const navigate = useNavigate();

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const signUpValidateFunction = async (): Promise<boolean> => {
    if (password != confirmPassword) {
      setErrorMessage("password və confirmPassword eyni olmalidilar!!!");
      return false;
    }

    if (confirmEmail) {
      if (emailVerifictionCode != verificationCode) {
        setErrorMessage("Email 6 Nomrəli Doğrulama Codu Səfdir!!!");
        return false;
      }
    } else {
      try {
        const responseEmail = await axios.get(
          `https://localhost:7275/auth/checkemail`,
          {
            params: { email: email },
          }
        );
        if (responseEmail.data) {
          setErrorMessage("Bu Email əvəlcə istifaidə olunub!!!");
          return false;
        }
      } catch (error) {
        console.error("Error during email validation", error);
      }
    }

    if (!userNameValidateFunction()) {
      return false;
    }

    setErrorMessage("");
    return true;
  };

  const userNameValidateFunction = (): boolean => {
    if (!userName.includes("_")) {
      setErrorMessage("UserName mütləq ən azi 1 _ simvolu olmalidi olmalidi");
      return false;
    }

    if (!/\d/.test(userName)) {
      setErrorMessage(
        "UserName mütləq ən azi 1 (1234567890) Rəqəm simvolu olmalidi olmalidi"
      );
      return false;
    }

    if (userName.length < 8) {
      setErrorMessage("UserName mütləq 8 simvol və ay cox olmalidi");
      return false;
    }

    if (!/^[a-zA-Z0-9_]+$/.test(userName)) {
      setErrorMessage(
        "UserName-də ancaq rəqəm,(english (abc)) hərf və _ istifadə edə bilərsiz"
      );
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!(await signUpValidateFunction())) {
      return;
    }

    if (!confirmEmail) {
      const UserNameInput = document.getElementById(
        "userName"
      ) as HTMLInputElement | null;
      const EmailInput = document.getElementById(
        "Email"
      ) as HTMLInputElement | null;
      const PasswordInput = document.getElementById(
        "password"
      ) as HTMLInputElement | null;
      const ConfirmInput = document.getElementById(
        "confirmPassword"
      ) as HTMLInputElement | null;

      const inputs = [UserNameInput, EmailInput, PasswordInput, ConfirmInput];

      inputs.forEach((input) => {
        if (input) {
          input.readOnly = true;
        }
      });

      try {
        const dto = {
          Email: email,
        };
        const responseSMTP = await axios.post(
          `https://localhost:7275/auth/checksmtp`,
          dto,
          {
            headers: {
              "Content-Type": "application/json",
            },
          }
        );
        const encryptionRandomNumber = responseSMTP.data;
        setVerificationCode(String(encryptionRandomNumber - 121212));
        setConfirmEmail(true);
      } catch (error) {
        console.error("Error during SMTP check", error);
        alert("SMTP check failed!");
      }
    } else {
      try {
        const dto = {
          UserName: userName,
          Email: email,
          Password: password,
        };
        await axios.post(`https://localhost:7275/auth/signup`, dto, {
          headers: {
            "Content-Type": "application/json",
          },
        });
        console.log("Ok Sign Up Successful");
        navigate("/SignIn");
      } catch (error) {
        console.error("Error during check", error);
      }
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
      <div className="card p-4 shadow" style={{ width: "400px" }}>
        <h3 className="text-center mb-4">Sign Up</h3>
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
              value={userName}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setUserName(e.target.value)
              }
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
              value={email}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setEmail(e.target.value)
              }
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
              value={password}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setPassword(e.target.value)
              }
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
          <div className="mb-3 position-relative">
            <label htmlFor="confirmPassword" className="form-label">
              Confirm Password
            </label>
            <input
              name="confirmPassword"
              id="confirmPassword"
              placeholder="Confirm Password"
              type={showConfirmPassword ? "text" : "password"}
              className="form-control"
              required
              value={confirmPassword}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setConfirmPassword(e.target.value)
              }
            />
            <button
              type="button"
              onClick={toggleConfirmPasswordVisibility}
              className="btn position-absolute top-50 end-0 translate-middle-y"
              style={{
                border: "none",
                position: "relative",
                bottom: "35px",
                fontSize: 18,
              }}
            >
              {showConfirmPassword ? "**" : "👁️"}
            </button>
          </div>

          {confirmEmail && (
            <div className="mb-3">
              <label htmlFor="confirmEmailCode" className="form-label">
                Confirm Email Code
              </label>
              <input
                name="confirmEmailCode"
                id="confirmEmailCode"
                placeholder="Enter confirmEmailCode"
                type="text"
                className="form-control"
                required
                value={emailVerifictionCode}
                onChange={(e: ChangeEvent<HTMLInputElement>) => {
                  setEmailVerificationCode(e.target.value);
                }}
              />
            </div>
          )}

          {errorMessage == "" ? (
            <></>
          ) : (
            <p style={{ color: "red" }}>{errorMessage}</p>
          )}

          <button type="submit" className="btn btn-primary w-100">
            {confirmEmail ? "Sign Up" : "Get Permission"}
          </button>
          <div className="mt-3 text-center">
            <small>
              Do you have an account? <a href="/SignIn">Sign In</a>
            </small>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
