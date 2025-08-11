import { Weather } from "./components/Weather";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/weather',
    element: <Weather />
  }
];

export default AppRoutes;
