import './App.css';
import { RouterProvider, createBrowserRouter, createRoutesFromElements, Route } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import Layout from './components/Layout';
import History from './pages/History';
import Match from './pages/Match';
import ErrorPage from './pages/ErrorPage';
import Member from './pages/Member';
import Donate from './pages/Donate';
function App() {
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<Layout />}>
        <Route index element={<Match />} />
        <Route path="donate" element={<Donate />} />
        <Route path="member" element={<Member />} />
        <Route path="history" element={<History />} />
        <Route path='*' element={<ErrorPage />} />
      </Route>
    )
  )
  return (
    <RouterProvider router={router} />
  );
}

export default App;
