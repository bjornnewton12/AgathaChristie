import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { fetchBook, addMovieAdaptation, type Book } from "../api/books";
import {
  fetchUserBooks,
  updateUserBook,
  type UserBook,
} from "../api/userbooks";
import { useAuth } from "../AuthContext";
import checkIcon from "../assets/icons/Icon_Check.svg";
import arrowLeftIcon from "../assets/icons/Icon_Arrow_Left_Bold.svg";
import editIcon from "../assets/icons/Icon_Edit.svg";
import plusIcon from "../assets/icons/Icon_Plus_Large.svg";
import "./BookDetailPage.css";

function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
}

export default function BookDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { token } = useAuth();
  const [book, setBook] = useState<Book | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [userBook, setUserBook] = useState<UserBook | null>(null);
  const [showReadPicker, setShowReadPicker] = useState(false);
  const [selectedDate, setSelectedDate] = useState("");
  const [showAddOverlay, setShowAddOverlay] = useState(false);
  const [tmdbUrlInput, setTmdbUrlInput] = useState("");
  const [addError, setAddError] = useState<string | null>(null);

  useEffect(() => {
    document.body.style.overscrollBehavior = "none";
    return () => {
      document.body.style.overscrollBehavior = "";
    };
  }, []);

  useEffect(() => {
    if (!id) return;
    fetchBook(id)
      .then(setBook)
      .catch(() => setError("Could not load book"));
  }, [id]);

  useEffect(() => {
    if (!token || !id) return;
    fetchUserBooks(token)
      .then((ubs) => setUserBook(ubs.find((ub) => ub.bookId === id) ?? null))
      .catch(() => {});
  }, [token, id]);

  async function toggleRead() {
    if (!token || !id) return;
    if (userBook?.isRead) {
      const isOwnedEnglish = userBook?.isOwnedEnglish ?? false;
      const isOwnedSwedish = userBook?.isOwnedSwedish ?? false;
      const updated: UserBook = {
        bookId: id,
        isRead: false,
        dateRead: null,
        isOwnedEnglish,
        isOwnedSwedish,
      };
      setUserBook(updated);
      await updateUserBook(
        id,
        { isRead: false, dateRead: null, isOwnedEnglish, isOwnedSwedish },
        token,
      );
    } else {
      setSelectedDate("");
      setShowReadPicker(true);
    }
  }

  async function confirmRead() {
    if (!token || !id) return;
    const dateRead = selectedDate ? new Date(selectedDate).toISOString() : null;
    const isOwnedEnglish = userBook?.isOwnedEnglish ?? false;
    const isOwnedSwedish = userBook?.isOwnedSwedish ?? false;
    const updated: UserBook = {
      bookId: id,
      isRead: true,
      dateRead,
      isOwnedEnglish,
      isOwnedSwedish,
    };
    setUserBook(updated);
    setShowReadPicker(false);
    await updateUserBook(
      id,
      { isRead: true, dateRead, isOwnedEnglish, isOwnedSwedish },
      token,
    );
  }

  async function toggleOwnedEnglish() {
    if (!token || !id) return;
    const isOwnedEnglish = !(userBook?.isOwnedEnglish ?? false);
    const isOwnedSwedish = userBook?.isOwnedSwedish ?? false;
    const isRead = userBook?.isRead ?? false;
    const dateRead = userBook?.dateRead ?? null;
    const updated: UserBook = {
      bookId: id,
      isRead,
      dateRead,
      isOwnedEnglish,
      isOwnedSwedish,
    };
    setUserBook(updated);
    await updateUserBook(
      id,
      { isRead, dateRead, isOwnedEnglish, isOwnedSwedish },
      token,
    );
  }

  async function toggleOwnedSwedish() {
    if (!token || !id) return;
    const isOwnedSwedish = !(userBook?.isOwnedSwedish ?? false);
    const isOwnedEnglish = userBook?.isOwnedEnglish ?? false;
    const isRead = userBook?.isRead ?? false;
    const dateRead = userBook?.dateRead ?? null;
    const updated: UserBook = {
      bookId: id,
      isRead,
      dateRead,
      isOwnedEnglish,
      isOwnedSwedish,
    };
    setUserBook(updated);
    await updateUserBook(
      id,
      { isRead, dateRead, isOwnedEnglish, isOwnedSwedish },
      token,
    );
  }

  function openAddOverlay() {
    setTmdbUrlInput("");
    setAddError(null);
    setShowAddOverlay(true);
  }

  function closeAddOverlay() {
    setShowAddOverlay(false);
  }

  async function submitAdaptation() {
    if (!token || !id) return;
    try {
      const created = await addMovieAdaptation(
        id,
        { tmdbUrl: tmdbUrlInput },
        token,
      );
      setBook((prev) =>
        prev
          ? { ...prev, movieAdaptations: [...prev.movieAdaptations, created] }
          : prev,
      );
      setShowAddOverlay(false);
    } catch {
      setAddError("Could not find that movie on TMDB");
    }
  }

  if (error) return <p className="page-error">{error}</p>;
  if (!book) return <p className="page-loading">Loading...</p>;

  const isRead = userBook?.isRead ?? false;
  const isOwnedEnglish = userBook?.isOwnedEnglish ?? false;
  const isOwnedSwedish = userBook?.isOwnedSwedish ?? false;
  const heroColor =
    isRead && isOwnedEnglish
      ? "#22C5BF"
      : isRead
        ? "#82D1A3"
        : isOwnedEnglish
          ? "#FFC25D"
          : "#FF7342";
  const dateRead = userBook?.dateRead ?? null;

  return (
    <div className="book-detail">
      <div className="book-detail-hero" style={{ backgroundColor: heroColor }}>
        <div className="book-detail-hero-nav">
          <Link to="/">
            <img src={arrowLeftIcon} alt="Back" />
          </Link>
          <Link to={`/books/${book.id}/edit`}>
            <img src={editIcon} alt="Edit" />
          </Link>
        </div>
        <h1 className="book-detail-title">{book.title}</h1>
        {book.titleSwedish && (
          <p className="book-detail-title-swedish">{book.titleSwedish}</p>
        )}
      </div>
      {showAddOverlay && (
        <div className="adaptation-overlay-backdrop" onClick={closeAddOverlay}>
          <div
            className="adaptation-add-box"
            style={{ backgroundColor: heroColor }}
            onClick={(e) => e.stopPropagation()}
          >
            <h2>Add movie</h2>
            <p>
              To add a movie, paste its link from tmdb.com in the field below:
            </p>
            <input
              type="text"
              placeholder="Add link here..."
              value={tmdbUrlInput}
              onChange={(e) => setTmdbUrlInput(e.target.value)}
            />
            {addError && <p className="adaptation-add-error">{addError}</p>}
            <div className="adaptation-add-actions">
              <button onClick={submitAdaptation}>Submit</button>
              <button onClick={closeAddOverlay}>Cancel</button>
            </div>
          </div>
        </div>
      )}
      <div className="book-detail-body">
        <p className="book-detail-meta">
          {book.releaseYear} | {book.genre.name}
          {book.detectives.length > 0 &&
            ` | ${book.detectives.map((d) => d.name).join(", ")}`}
        </p>
        <div className="book-detail-toggles">
          <button className="book-detail-toggle" onClick={toggleRead}>
            <span className="toggle-icon-box">
              {isRead && (
                <img src={checkIcon} alt="" className="toggle-icon-img" />
              )}
            </span>
            <span>
              {isRead && dateRead
                ? `You read it on ${formatDate(dateRead)}`
                : isRead
                  ? "You have read it"
                  : "Have you read this book?"}
            </span>
          </button>
          {showReadPicker && (
            <div className="read-date-picker">
              <input
                type="date"
                value={selectedDate}
                onChange={(e) => setSelectedDate(e.target.value)}
              />
              <button onClick={confirmRead}>Mark as read</button>
            </div>
          )}
          <button className="book-detail-toggle" onClick={toggleOwnedEnglish}>
            <span className="toggle-icon-box">
              {isOwnedEnglish && (
                <img src={checkIcon} alt="" className="toggle-icon-img" />
              )}
            </span>
            <span>
              {isOwnedEnglish
                ? "You own the English version"
                : "Do you own the English version?"}
            </span>
          </button>
          <button className="book-detail-toggle" onClick={toggleOwnedSwedish}>
            <span className="toggle-icon-box">
              {isOwnedSwedish && (
                <img src={checkIcon} alt="" className="toggle-icon-img" />
              )}
            </span>
            <span>
              {isOwnedSwedish
                ? "You own the Swedish version"
                : "Do you own the Swedish version?"}
            </span>
          </button>
        </div>
        {book.synopsis && (
          <section>
            <h2>Synopsis</h2>
            <p>{book.synopsis}</p>
          </section>
        )}
        {book.trivia.length > 0 && (
          <section>
            <h2>Trivia</h2>
            <ul className="trivia-list">
              {book.trivia.map((item, i) => (
                <li key={i} className="trivia-item-view">
                  {item}
                </li>
              ))}
            </ul>
          </section>
        )}

        <section>
          <h2>Movie adaptations</h2>
          <div className="adaptation-scroll">
            {book.movieAdaptations.map((m) => (
              <div key={m.id} className="adaptation-card">
                {m.posterImageUrl && (
                  <img src={m.posterImageUrl} alt={m.title} />
                )}
                <p className="adaptation-title">{m.title}</p>
                <p className="adaptation-year">{m.releaseYear}</p>
              </div>
            ))}
            {token && (
              <button
                className="adaptation-add-trigger"
                onClick={openAddOverlay}
              >
                <img src={plusIcon} alt="" />
                <span>Add movie</span>
              </button>
            )}
          </div>
        </section>
      </div>
    </div>
  );
}
