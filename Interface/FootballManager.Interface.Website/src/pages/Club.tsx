import { Col, Row, Image } from "react-bootstrap";

function Club() {
    return (<>
        <Row>
            <Col>
                <Row className="justify-content-center">
                    <img style={{ width: "200px" }} src=".\logo.jpg" />
                </Row>
                <Row className="justify-content-center">
                    <img style={{ width: "50px" }} src=".\logo.jpg" />
                </Row>
                <Row className="justify-content-center">5th</Row>
                <Row className="justify-content-center">Premier Division</Row>
            </Col>
            <Col></Col>
        </Row>
    </>
    )
}
export default Club;