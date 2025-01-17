import axios from "axios";
import { ChangeEvent, FormEvent, useEffect, useState } from "react";

const HomePage = () => {
  const [errorAddMp3Message, setErrorAddMp3Message] = useState<string>("");
  const [mp3Name, setMp3Name] = useState<string>("");
  const [mp3Image, setMp3Image] = useState<File>();
  const [mp3Sound, setMp3Sound] = useState<File>();

  const [token, setToken] = useState<string>("");
  const [userId, setUserId] = useState<string>("");
  const [mp3List, setMp3List] = useState<GetMp3Dto[]>([]);

  const [selectedMenu, setSelectedMenu] = useState<string>("Popular");
  const [searchMp3Input, setSearchMp3Input] = useState<string>("");

  const [commentMp3Input, setCommentMp3Input] = useState<string>("");
  const [selectedMp3Id, setSelectedMp3Id] = useState<number>(-1);
  const [selectedMp3Name, setSelectedMp3Name] = useState<string>("");

  const [commentList, setCommentList] = useState<GetCommentDto[]>([]);

  interface GetMp3Dto {
    id: number;
    name: string;
    likeCount: number;
    imageUrl?: string;
    soundUrl?: string;
    favorite: boolean;
    ownerName?: string;
  }

  interface Mp3Token {
    Token: string;
    UserId: string;
    Date: Date;
    UserName: string;
    Email: string;
    LikeLimit: number;
  }

  interface GetCommentDto {
    ownerUserName: string;
    comment: string;
    dateTime: string;
  }

  const getComments = async (mp3Id: number) => {
    try {
      console.log(mp3Id);

      const response = await axios.get(
        "http://localhost:5001/mp3/mp3comments",
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
          params: {
            mp3Id: mp3Id,
          },
        }
      );

      console.dir(response);
      setCommentList(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  const AddCommentMp3Function = async () => {
    try {
      const dto = {
        UserId: userId,
        Mp3Id: selectedMp3Id,
        Comment: commentMp3Input,
      };

      await axios.post("http://localhost:5001/mp3/addcomment", dto, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const commentMp3InputTeg = document.getElementById(
        "commentMp3Input"
      ) as HTMLInputElement;
      commentMp3InputTeg.value = "";
      setCommentMp3Input("");

      getComments(selectedMp3Id);
    } catch (error) {
      console.log(error);
    }
  };

  const handleSearchMp3 = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      const response = await axios.get("http://localhost:5001/mp3/mp3s", {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
        params: { userId: userId, mp3Name: searchMp3Input },
      });

      setMp3List(response.data);
      const menuItems = ["menuItemPopular", "menuItemFavorites", "menuItemAll"];
      menuItems.forEach((itemId) => {
        const menuItem = document.getElementById(itemId) as HTMLElement;
        if (menuItem) {
          menuItem.style.color = "black";
          menuItem.style.backgroundColor = "white";
        }
      });
    } catch (error) {
      console.log(error);
    }
  };

  const addLikeMp3Function = async (mp3Id: number) => {
    try {
      const mp3TokenObjString = localStorage.getItem("mp3TokenObj");

      if (mp3TokenObjString) {
        const mp3TokenObj = JSON.parse(mp3TokenObjString) as Mp3Token;

        if (mp3TokenObj.LikeLimit != 0) {
          await axios.post("http://localhost:5001/mp3/addlikemp3", null, {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "multipart/form-data",
            },
            params: { mp3Id: mp3Id },
          });

          selectMenuFunction(selectedMenu);
          mp3TokenObj.LikeLimit = mp3TokenObj.LikeLimit - 1;
          localStorage.setItem("mp3TokenObj", JSON.stringify(mp3TokenObj));
        } else {
          alert("Sizin Like Limitini bitib size limit gun icinde verilecek");
        }
      }
    } catch (erroe) {
      console.log(erroe);
    }
  };

  const addFavoriteMp3Function = async (mp3Id: number) => {
    try {
      await axios.post("http://localhost:5001/mp3/changefavorite", null, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
        params: {
          mp3Id: mp3Id,
          userId: userId,
        },
      });

      selectMenuFunction(selectedMenu);
    } catch (error) {
      console.log(error);
    }
  };

  const selectMenuFunction = async (selectedName: string) => {
    setSelectedMenu(selectedName);

    const menuItems = [
      "menuItemPopular",
      "menuItemFavorites",
      "menuItemAll",
      "menuItemMyMp3",
    ];
    menuItems.forEach((itemId) => {
      const menuItem = document.getElementById(itemId) as HTMLElement;
      if (menuItem) {
        menuItem.style.color = "black";
        menuItem.style.backgroundColor = "white";
      }
    });

    const selectedMenuItem = document.getElementById(
      `menuItem${selectedName}`
    ) as HTMLElement;
    if (selectedMenuItem) {
      selectedMenuItem.style.color = "white";
      selectedMenuItem.style.backgroundColor = "#007bff";
    }

    try {
      switch (selectedName) {
        case "Popular":
          try {
            const response = await axios.get(
              "http://localhost:5001/mp3/popular",
              {
                headers: {
                  Authorization: `Bearer ${token}`,
                  "Content-Type": "multipart/form-data",
                },
                params: {
                  userId: userId,
                },
              }
            );
            console.dir(response);
            setMp3List(response.data);
          } catch (error) {
            console.log("Error Favorite Mp3  " + error);
          }
          break;
        case "Favorites":
          try {
            const response = await axios.get(
              "http://localhost:5001/mp3/favorites",
              {
                headers: {
                  Authorization: `Bearer ${token}`,
                  "Content-Type": "multipart/form-data",
                },
                params: {
                  userId: userId,
                },
              }
            );
            console.dir(response);
            setMp3List(response.data);
          } catch (error) {
            console.log("Error Favorite Mp3  " + error);
          }
          break;
        case "MyMp3":
          try {
            const response = await axios.get(
              "http://localhost:5001/mp3/mymp3s",
              {
                headers: {
                  Authorization: `Bearer ${token}`,
                  "Content-Type": "multipart/form-data",
                },
                params: {
                  userId: userId,
                },
              }
            );
            console.dir(response);
            setMp3List(response.data);
          } catch (error) {
            console.log("Error Favorite Mp3  " + error);
          }
          break;
        case "All":
          try {
            const response = await axios.get("http://localhost:5001/mp3/mp3s", {
              headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "multipart/form-data",
              },
              params: {
                userId: userId,
              },
            });
            console.dir(response);
            setMp3List(response.data);
          } catch (error) {
            console.log("Error Favorite Mp3  " + error);
          }
          break;
        default:
          break;
      }
    } catch (error) {
      console.error("Error fetching MP3 data:", error);
    }
  };

  const getFavoriteMp3s = async () => {
    const mp3TokenObjString = localStorage.getItem("mp3TokenObj");

    if (mp3TokenObjString) {
      const mp3TokenObj = JSON.parse(mp3TokenObjString) as Mp3Token;

      setToken(mp3TokenObj.Token);
      setUserId(mp3TokenObj.UserId);

      try {
        const response = await axios.get("http://localhost:5001/mp3/popular", {
          headers: {
            Authorization: `Bearer ${mp3TokenObj.Token}`,
            "Content-Type": "multipart/form-data",
          },
          params: {
            userId: mp3TokenObj.UserId,
          },
        });

        console.dir(response);

        setMp3List(response.data);
      } catch (error) {
        console.log("Error Favorite Mp3  " + error);
      }
    }
  };

  useEffect(() => {
    getFavoriteMp3s();
    const commentMp3Input = document.getElementById(
      "commentMp3Input"
    ) as HTMLInputElement;
    const commentMp3Button = document.getElementById(
      "commentMp3Button"
    ) as HTMLButtonElement;

    commentMp3Input.disabled = true;
    commentMp3Button.disabled = true;
  }, []);

  const handleAddMp3Submit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const handleAddMp3 = async () => {
      try {
        const formData = new FormData();

        formData.append("UserId", userId);
        formData.append("Name", mp3Name);

        if (mp3Image) {
          formData.append("ImageFile", mp3Image);
        } else {
          console.error("No image file selected");
        }

        if (mp3Sound) {
          formData.append("Mp3File", mp3Sound);
        } else {
          console.error("No mp3 file selected");
        }

        const response = await axios.post(
          "http://localhost:5001/mp3/add",
          formData,
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "multipart/form-data",
            },
          }
        );

        setMp3Name("");
        setMp3Image(undefined);
        setMp3Sound(undefined);
        const mp3ImageVariable = document.getElementById(
          "mp3Image"
        ) as HTMLInputElement;

        const mp3SoundVariable = document.getElementById(
          "mp3Sound"
        ) as HTMLInputElement;

        mp3SoundVariable.value = "";

        mp3ImageVariable.value = "";

        selectMenuFunction(selectedMenu);

        console.log("MP3 Added:", response.data);
      } catch (error) {
        console.error("Error adding MP3:", error);
      }
    };

    handleAddMp3();
  };
  return (
    <div className="container-fluid">
      <div className="row" style={{ height: "100vh" }}>
        <div
          className="col-md-3 col-lg-2"
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            height: "100vh",
            overflowY: "auto",
            display: "flex",
            justifyContent: "left",
            padding: 0,
            overflowX: "hidden",
            whiteSpace: "nowrap",
            width: "220px",
          }}
        >
          <div className="card shadow-lg">
            <div className="card-body" style={{ width: 400 }}>
              <h3
                className="text-center"
                style={{
                  position: "relative",
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  width: "50%",
                  paddingBottom: 10,
                }}
              >
                MP3 Menu
              </h3>

              <nav className="nav flex-column">
                <a
                  id="menuItemPopular"
                  className="nav-link"
                  style={{
                    color: "white",
                    backgroundColor: "#007bff",
                    padding: 10,
                    fontSize: 20,
                    display: "flex",
                    justifyContent: "center",
                    borderRadius: 5,
                    width: "50%",
                    cursor: "pointer",
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    selectMenuFunction("Popular");
                  }}
                >
                  Popular
                </a>
                <a
                  id="menuItemFavorites"
                  className="nav-link"
                  style={{
                    color: "black",
                    padding: 10,
                    fontSize: 20,
                    display: "flex",
                    justifyContent: "center",
                    borderRadius: 5,
                    width: "50%",
                    cursor: "pointer",
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    selectMenuFunction("Favorites");
                  }}
                >
                  Favorites
                </a>
                <a
                  id="menuItemMyMp3"
                  className="nav-link"
                  style={{
                    color: "black",
                    padding: 10,
                    fontSize: 20,
                    display: "flex",
                    justifyContent: "center",
                    borderRadius: 5,
                    width: "50%",
                    cursor: "pointer",
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    selectMenuFunction("MyMp3");
                  }}
                >
                  My Mp3
                </a>
                <a
                  id="menuItemAll"
                  className="nav-link"
                  href="#"
                  style={{
                    color: "black",
                    padding: 10,
                    fontSize: 20,
                    display: "flex",
                    justifyContent: "center",
                    borderRadius: 5,
                    width: "50%",
                    cursor: "pointer",
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    selectMenuFunction("All");
                  }}
                >
                  All
                </a>

                <button
                  style={{
                    color: "white",
                    background: "red",
                    padding: 10,
                    fontSize: 20,
                    display: "flex",
                    justifyContent: "center",
                    borderRadius: 5,
                    border: 0,
                    width: "50%",
                    marginTop: 20,
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    const confirmation = window.confirm(
                      "Are you sure you want to log out?"
                    );
                    if (confirmation) {
                      localStorage.removeItem("mp3TokenObj");
                      document.location.href = "/";
                    }
                  }}
                >
                  Log Out
                </button>
              </nav>
            </div>
          </div>
        </div>

        <div
          style={{
            marginLeft: 215,
            marginRight: "auto",

            padding: "30px 20px",

            width: 1130,
            position: "relative",
          }}
        >
          <div
            style={{
              position: "fixed",
              top: 0,
              left: 225.8,
              zIndex: 2,
              marginTop: 15,
            }}
          >
            <form
              style={{
                display: "flex",
                justifyContent: "start",
                alignItems: "center",
                padding: 10,

                gap: 10,
              }}
              onSubmit={handleSearchMp3}
            >
              <input
                name="searchMp3Input"
                id="searchMp3Input"
                placeholder="Search..."
                type="text"
                className="form-control"
                required
                style={{ width: 300 }}
                value={searchMp3Input}
                onChange={(e: ChangeEvent<HTMLInputElement>) => {
                  if (e.target.value.length > 23) {
                    return;
                  } else {
                    setSearchMp3Input(e.target.value);
                  }
                }}
              />
              <button
                style={{
                  borderRadius: 3,
                  border: "0",

                  padding: "6px 20px",
                  color: "white",
                  background: "#0d6efd",
                  fontWeight: "revert",
                }}
                type="submit"
              >
                Search
              </button>
            </form>
          </div>

          <div className="row g-4">
            {mp3List.map((mp3) => (
              <div
                className="col-md-4 col-lg-3"
                key={mp3.id}
                style={{
                  width: "275px",
                  height: "302.5px",
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  position: "relative",
                  paddingTop: 100,

                  gap: 0,
                }}
              >
                <div
                  className="card"
                  style={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                  }}
                >
                  <div
                    className="position-absolute top-0 end-0 m-2"
                    style={{
                      cursor: "pointer",
                      fontSize: "24px",
                    }}
                    onClick={() => addFavoriteMp3Function(mp3.id)}
                  >
                    <img
                      src={`./public/heart_${mp3.favorite ? "on" : "off"}.png`}
                      alt={`heart_${mp3.favorite ? "on" : "off"}`}
                      style={{ height: 45, width: 45 }}
                    />
                  </div>

                  <div
                    style={{
                      height: 170,
                      width: 170,
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      position: "relative",
                      top: 15,
                    }}
                  >
                    <img
                      src={mp3.imageUrl}
                      className="card-img-top"
                      alt={mp3.name}
                      style={{
                        padding: "15px 5px",
                        objectFit: "cover",
                      }}
                    />
                  </div>

                  <div className="card-body">
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "center",
                        gap: 10,
                      }}
                    >
                      <h6
                        className="card-title"
                        style={{ width: 100, height: 40 }}
                      >
                        {mp3.name}
                      </h6>
                      <div>
                        <button
                          onClick={() => {
                            addLikeMp3Function(mp3.id);
                          }}
                          style={{
                            fontSize: 12,
                            fontWeight: "bold",
                            position: "relative",
                            top: 2,
                            border: "1px solid darkgray",
                            borderRadius: 3,
                            height: 25,
                          }}
                        >
                          Like Count: {mp3.likeCount}
                        </button>
                        <p
                          style={{
                            color: "#0d6efd",
                            fontSize: 10,
                            cursor: "pointer",
                            position: "relative",
                            top: 10,
                            left: 12,
                          }}
                          onClick={() => {
                            setSelectedMp3Id(mp3.id);
                            setSelectedMp3Name(mp3.name);
                            const commentMp3Input = document.getElementById(
                              "commentMp3Input"
                            ) as HTMLInputElement;
                            const commentMp3Button = document.getElementById(
                              "commentMp3Button"
                            ) as HTMLButtonElement;

                            commentMp3Input.disabled = false;
                            commentMp3Button.disabled = false;
                            getComments(mp3.id);
                          }}
                        >
                          Comments {">>"}
                        </p>
                      </div>
                    </div>

                    <div
                      style={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                      }}
                    >
                      <audio
                        controls
                        className=""
                        style={{
                          width: "350px",
                          height: "50px",
                          transform: "scale(0.6)",
                        }}
                      >
                        <source src={mp3.soundUrl} type="audio/mpeg" />
                      </audio>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div
          className="col-md-3"
          style={{
            position: "fixed",
            top: 0,
            right: 0,
            height: "100vh",
            padding: 0,
            width: "48vh",
          }}
        >
          <div className="card shadow-lg p-4">
            <h3 className="mb-4 text-center" style={{}}>
              Add MP3
            </h3>
            <form
              onSubmit={handleAddMp3Submit}
              style={{
                height: "100vh",
              }}
            >
              <div className="mb-3">
                <label htmlFor="mp3Name" className="form-label">
                  MP3 Name
                </label>
                <input
                  name="mp3Name"
                  id="mp3Name"
                  placeholder="Enter MP3 Name"
                  type="text"
                  className="form-control"
                  required
                  value={mp3Name}
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    if (e.target.value.length > 23) {
                      setErrorAddMp3Message(
                        "Mp3 Name Maksimum 20 simvol ola biler"
                      );
                      return;
                    } else {
                      setErrorAddMp3Message("");
                      setMp3Name(e.target.value);
                    }
                  }}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="mp3Image" className="form-label">
                  MP3 Image
                </label>
                <input
                  name="mp3Image"
                  id="mp3Image"
                  type="file"
                  className="form-control"
                  accept="image/*"
                  required
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    const file = e.target.files?.[0];
                    if (file) {
                      if (file.type.startsWith("image/")) {
                        setMp3Image(file);
                        setErrorAddMp3Message("");
                      } else {
                        setErrorAddMp3Message(
                          "MP3 Image should be an image file."
                        );
                        const mp3ImageVariable = document.getElementById(
                          "mp3Image"
                        ) as HTMLInputElement;

                        mp3ImageVariable.value = "";
                      }
                    }
                  }}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="mp3Sound" className="form-label">
                  MP3 Sound
                </label>
                <input
                  name="mp3Sound"
                  id="mp3Sound"
                  type="file"
                  className="form-control"
                  required
                  accept="audio/mpeg"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    const file = e.target.files?.[0];
                    if (file) {
                      if (file.name.endsWith(".mp3")) {
                        setMp3Sound(file);
                        setErrorAddMp3Message("");
                      } else {
                        setErrorAddMp3Message(
                          "Only MP3 sound files are allowed."
                        );
                        setMp3Sound(undefined);
                        const mp3SoundVariable = document.getElementById(
                          "mp3Sound"
                        ) as HTMLInputElement;

                        mp3SoundVariable.value = "";
                      }
                    }
                  }}
                />
              </div>

              {errorAddMp3Message && (
                <p style={{ color: "red" }}>{errorAddMp3Message}</p>
              )}

              <button type="submit" className="btn btn-primary w-100">
                Add MP3
              </button>
            </form>
          </div>

          <div
            style={{
              padding: "10px",
              position: "absolute",
              flexDirection: "column",
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              zIndex: 3,
              top: 400,
            }}
          >
            <h3 style={{ marginBottom: "10px", color: "Black" }}>Comments</h3>
            <p>
              {">>"} {selectedMp3Name} {"<<"}
            </p>
            <div
              style={{
                maxHeight: "230px",
                overflowY: "scroll",
                width: 365,
                paddingRight: "10px",
                marginRight: "-5px",
                boxSizing: "content-box",
              }}
            >
              {commentList.map((item, index) => (
                <div
                  key={index}
                  style={{
                    marginBottom: "10px",
                    padding: "10px",
                    border: "1px solid #ddd",
                    borderRadius: "5px",
                    backgroundColor: "#f9f9f9",
                  }}
                >
                  <strong style={{ color: "#0d6efd" }}>
                    {item.ownerUserName} - {item.dateTime}
                  </strong>
                  <p style={{ margin: "5px 0" }}>{item.comment}</p>
                </div>
              ))}
            </div>
          </div>

          <div>
            <form
              onSubmit={(e) => {
                e.preventDefault();
                AddCommentMp3Function();
              }}
              style={{
                display: "flex",
                justifyContent: "start",
                alignItems: "center",
                padding: 10,

                gap: 10,
                position: "absolute",
                top: 750,
                zIndex: 4,
              }}
            >
              <input
                name="commentMp3Input"
                id="commentMp3Input"
                placeholder="Search..."
                type="text"
                className="form-control"
                required
                style={{ width: 270, opacity: selectedMp3Id === -1 ? 0.5 : 1 }}
                value={commentMp3Input}
                onChange={(e: ChangeEvent<HTMLInputElement>) => {
                  if (e.target.value.length > 35) {
                    return;
                  } else {
                    setCommentMp3Input(e.target.value);
                  }
                }}
              />
              <button
                id="commentMp3Button"
                style={{
                  borderRadius: 5,
                  border: "0",
                  opacity: selectedMp3Id === -1 ? 0.5 : 1,
                  cursor: selectedMp3Id === -1 ? "not-allowed" : "pointer",
                  padding: "5px 20px",
                  color: "white",
                  background: "#0d6efd",
                  fontWeight: "revert",
                }}
                type="submit"
              >
                Send
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
