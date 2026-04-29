import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import BookListPage from './pages/BookListPage'
import BookDetailPage from './pages/BookDetailPage'
import BookFormPage from './pages/BookFormPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<BookListPage />} />
          <Route path="books/:id" element={<BookDetailPage />} />
          <Route path="books/new" element={<BookFormPage />} />
          <Route path="books/:id/edit" element={<BookFormPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
