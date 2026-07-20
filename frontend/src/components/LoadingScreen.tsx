import "./LoadingScreen.css";

export default function LoadingScreen() {
  return (
    <div className="book-list">
      <img
        src="/logo/Logo.svg"
        alt="Agatha Christie"
        className="book-list-logo"
      />
      <p className="book-stats">
        Loading books,
        <br />
        this will take some time
      </p>
      <div className="loading-dots">
        <span />
        <span />
        <span />
      </div>
    </div>
  );
}
